using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TenEightVideo.Web.Data;
using TenEightVideo.Web.Mail;

namespace TenEightVideo.JobRunner.WarrantyReports
{
    public enum ScheduleInterval
    {
        Daily = 1,
        Monthly = 2,
        Yearly = 4
    }

    public class WarrantyScheduleProcessor : IWarrantyScheduleProcessor
    {
        private const int HOURS_PER_DAY = 24;
        private const int HOURS_PER_MONTH = 730;
        private const int HOURS_PER_YEAR = 8760;
        private IProcessScheduleRepository _processScheduleRepository;
        private IWarrantyRequestPartRepository _warrantyRequestPartRepository;
        private IMailManager _mailManager;

        public WarrantyScheduleProcessor(IProcessScheduleRepository processScheduleRepository, IWarrantyRequestPartRepository warrantyRequestPartRepository, IMailManager mailManager)
        {
            _processScheduleRepository = processScheduleRepository;
            _warrantyRequestPartRepository = warrantyRequestPartRepository;
            _mailManager = mailManager;
        }

        public bool ProcessMonthlyWarrantyReport(MailAddress sender, MailAddress recipient, MailAddress? bcc)
        {
            var processName = "Monthly Warranty Report";
            var emailSubject = "Monthly Website Warranty Part Request Report";
            var sent = ProcessWarrantyReport(processName, ScheduleInterval.Monthly, sender, recipient, emailSubject, bcc);
            return sent;
        }

        public bool ProcessYearlyWarrantyReport(MailAddress sender, MailAddress recipient, MailAddress? bcc)
        {
            var processName = "Yearly Warranty Report";
            var emailSubject = "Yearly Website Warranty Part Request Report";
            var sent = ProcessWarrantyReport(processName, ScheduleInterval.Yearly, sender, recipient, emailSubject, bcc);
            return sent;
        }

        private bool ProcessWarrantyReport(string processName, ScheduleInterval interval, MailAddress sender, MailAddress recipient, string emailSubject, MailAddress bcc)
        {
            var sent = false;
            var now = DateTime.Now;

            var schedule = _processScheduleRepository.GetByNameAndSchedule(processName, (int)interval);
            if (schedule == null)
            {
                schedule = new ProcessSchedule()
                {
                    ProcessName = processName,
                    Schedule = (int)interval,
                    DateCreated = DateTime.Now,
                    DateLastProcessed = now
                };
                _processScheduleRepository.Add(schedule);
            }

            int hoursToAdd;
            switch (interval)
            {
                case ScheduleInterval.Daily:
                    hoursToAdd = HOURS_PER_DAY;
                    break;
                case ScheduleInterval.Monthly:
                    hoursToAdd = HOURS_PER_MONTH;
                    break;
                case ScheduleInterval.Yearly:
                    hoursToAdd = HOURS_PER_YEAR;
                    break;
                default:
                    throw new InvalidOperationException("Unhandled schedule interval.");
            }

            var dateLastProcessed = (schedule.DateLastProcessed == null) ? now.AddHours(-hoursToAdd) : schedule.DateLastProcessed.Value;

            if (DateTime.Now.Subtract(dateLastProcessed).TotalHours > hoursToAdd)
            {
                IEnumerable<WarrantyPartCountDataRecord> records = GetWarrantyRecords(dateLastProcessed, now);
                var info = new WarrantyReportInfo(emailSubject)
                {                    
                    PeriodStartDate = dateLastProcessed,
                    PeriodEndDate = now,
                    Records = records
                };
                _mailManager.SendWarrantyReport(sender, recipient, info, bcc);
                sent = true;
                schedule.DateLastProcessed = now;
                _processScheduleRepository.Update(schedule);
            }
            return sent;
        }

        private IEnumerable<WarrantyPartCountDataRecord> GetWarrantyRecords(DateTime startDate, DateTime endDate)
        {
            var forms = _warrantyRequestPartRepository.GetAll(p => p.WarrantyRequest.DateCreated >= startDate && p.WarrantyRequest.DateCreated <= endDate);
            var partGroups = forms.GroupBy(f => f.PartRequested);
            var records = partGroups.Select(g => new WarrantyPartCountDataRecord() { PartRequested = g.Key, Count = g.Sum(i => i.Quantity) });
            return records;
        }
    }
}
