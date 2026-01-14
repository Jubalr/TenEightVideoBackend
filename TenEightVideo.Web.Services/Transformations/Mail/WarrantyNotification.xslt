<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="html" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="warrantyNotificationInfo">
    <html xmlns="http://www.w3.org/1999/xhtml" >
      <head>
        <title>Warranty Request Notification</title>
      </head>
      <body>
        <table style="width:600px;">
          <tr>
            <td style="font-weight:bold;width:200px;">
              Warranty Request Id
            </td>
            <td>
              <xsl:value-of select="requestId"/>
            </td>
          </tr>
          <tr>
            <td style="font-weight:bold;width:200px;">
              Company
            </td>
            <td>
              <xsl:value-of select="company"/>
            </td>            
          </tr>
          <tr>
            <td style="font-weight:bold;width:200px;">
              First Name
            </td>
            <td>
              <xsl:value-of select="firstName"/>
            </td>
          </tr>
          <tr>
            <td style="font-weight:bold;width:200px;">
              Last Name
            </td>
            <td>
              <xsl:value-of select="lastName"/>
            </td>
          </tr>
          <tr>
            <td style="font-weight:bold;width:200px;">
              E-mail Address
            </td>
            <td>
              <xsl:element name="a">
                <xsl:attribute name="href">
                  mailto:<xsl:value-of select="emailAddress"/>
                </xsl:attribute>
                <xsl:value-of select="emailAddress"/>
              </xsl:element>              
            </td>
          </tr>
          <tr>
            <td style="font-weight:bold;width:200px;">
              Phone Number
            </td>
            <td>
              <xsl:value-of select="phoneNumber"/>
            </td>
          </tr>
          <tr>
            <td style="font-weight:bold;width:200px;">
              Serial Number
            </td>
            <td>
              <xsl:value-of select="serialNumber"/>
            </td>
          </tr>
          <tr>
            <td style="font-weight:bold;width:200px;">
              Address 1
            </td>
            <td>
              <xsl:value-of select="address1"/>
            </td>
          </tr>
          <tr>
            <td style="font-weight:bold;width:200px;">
              Address 2
            </td>
            <td>
              <xsl:value-of select="address2"/>
            </td>
          </tr>
          <tr>
            <td style="font-weight:bold;width:200px;">
              City
            </td>
            <td>
              <xsl:value-of select="city"/>
            </td>
          </tr>
          <tr>
            <td style="font-weight:bold;width:200px;">
              State / Province / Region
            </td>
            <td>
              <xsl:value-of select="state"/>
            </td>
          </tr>
          <tr>
            <td style="font-weight:bold;width:200px;">
              Postal / Zip Code
            </td>
            <td>
              <xsl:value-of select="zipCode"/>
            </td>
          </tr>
          <tr>
            <td style="font-weight:bold;width:200px;">
              Country
            </td>
            <td>
              <xsl:value-of select="country"/>
            </td>
          </tr>
          <tr>
            <td style="font-weight:bold;width:200px;vertical-align:top;">
              Parts Requested
            </td>
            <td>
              <table>
                <tr>
                  <td style="background-color:#ccc;border:solid 1px #ccc;font-weight:bold;padding:5px;">Name</td>
                  <td style="background-color:#ccc;border:solid 1px #ccc;font-weight:bold;padding:5px;">Qty</td>
                </tr>
                <xsl:for-each select="warrantyParts/part">
                  <tr>
                    <td style="border:solid 1px #ccc;padding:5px;"><xsl:value-of select="name"/></td>
                    <td style="border:solid 1px #ccc;padding:5px;"><xsl:value-of select="qty"/></td>
                  </tr>                    
                </xsl:for-each>
              </table>
            </td>
          </tr>
          <tr>
            <td style="font-weight:bold;width:200px;">
              Problem Description
            </td>
            <td>
              <xsl:value-of select="problemDescription"/>
            </td>
          </tr>
          <tr>
            <td style="font-weight:bold;width:200px;">
              Terms Acceptance
            </td>
            <td>
              <xsl:value-of select="termsAcceptance"/>
            </td>
          </tr>
        </table>               
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
