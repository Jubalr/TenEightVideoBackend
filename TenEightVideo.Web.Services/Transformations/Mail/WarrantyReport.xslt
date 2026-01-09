<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="html" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="warrantyReportInfo">
    <html xmlns="http://www.w3.org/1999/xhtml" >
      <head>
        <title>
          <xsl:value-of select="title"/>
        </title>
      </head>
      <body>

        <table cellpadding="5">
           <tr>
            <td style="font-weight:bold;width:200px;">
              Period Start Date:
            </td>
            <td>
              <xsl:value-of select="periodStartDate"/>
            </td>
          </tr>
          <tr>
            <td style="font-weight:bold;width:200px;">
              Period End Date:
            </td>
            <td>
              <xsl:value-of select="periodEndDate"/>
            </td>            
          </tr>
        </table>
        
        <xsl:if test="count(records/record) = 0">
          <p>
            There were no warranty requests recorded for this period.  
          </p>          
        </xsl:if>

        <xsl:if test="count(records/record) > 0">
          <p>
            Please find the <xsl:value-of select="title" /> below.
          </p>

          <table style="width:600px;" cellpadding="5">
            <tr style="background-color:#efefef">
              <td style="font-weight:bold;width:500px;">
                Part Requested
              </td>
              <td style="font-weight:bold;width:100px;text-align:right;">
                Count
              </td>
            </tr>
            <xsl:for-each select="records/record">
              <tr>
                <td>
                  <xsl:value-of select="partRequested"/>
                </td>
                <td style="text-align:right;">
                  <xsl:value-of select="count"/>
                </td>
              </tr>
            </xsl:for-each>
            <tr style="background-color:#efefef">
              <td style="font-weight:bold;width:500;">
                Total Count
              </td>
              <td style="font-weight:bold;width:100px;text-align:right;">
                <xsl:value-of select="totalCount"/>
              </td>
            </tr>
          </table>
        </xsl:if>

        
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
