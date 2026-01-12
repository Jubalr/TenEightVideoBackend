using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenEightVideo.JobRunner
{
    public class JobStatus
    {
        private const int SUCCESS = 0;
        private const int FAILURE = 1;

        private JobStatus(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public static JobStatus Success
        {
            get { return new JobStatus(SUCCESS); }
        }
        public static JobStatus Failure
        {
            get { return new JobStatus(FAILURE); }
        }

        public static implicit operator int(JobStatus status)
        {
            return status.Value;
        }

        public static implicit operator JobStatus(int value)
        {
            return new JobStatus(value);
        }        
    }
}
