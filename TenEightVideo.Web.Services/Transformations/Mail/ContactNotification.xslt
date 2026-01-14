<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="html" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="contactNotificationInfo">
    <html xmlns="http://www.w3.org/1999/xhtml" >
      <head>
        <title>Contact Notification</title>
      </head>
      <body>
        <p>
          <b>Inquiry Type: </b>
          <xsl:value-of select="inquiryType"/>
        </p>
        <p>
          <b>Agency/Department: </b>
          <xsl:value-of select="agencyOrDepartment"/>
        </p>
        <p>
          <b>State: </b>
          <xsl:value-of select="state"/>
        </p>
        <p>
          <b>Contact Name: </b>
          <xsl:value-of select="name"/>
        </p>
        <p>
          <b>Job Title/Rank: </b>
          <xsl:value-of select="jobTitle"/>
        </p>
        <p>
          <b>E-mail Address: </b>
          <xsl:element name="a">
            <xsl:attribute name="href">
              mailto:<xsl:value-of select="emailAddress"/>
            </xsl:attribute>
            <xsl:value-of select="emailAddress"/>
          </xsl:element>
        </p>
        <p>
          <b>Phone Number: </b>
          <xsl:value-of select="phoneNumber"/>
        </p>
        <p>
          <b>Preferred Contact Method: </b>
          <xsl:value-of select="preferredContactMethod"/>
        </p>
        <p>
          <b>Systems of Interest: </b>
          <xsl:value-of select="systemsOfInterest"/>
        </p>
        <p>
          <b>PDF Specs Requested: </b>
          <xsl:value-of select="pdfSpecsRequested"/>
        </p>
        <p>
          <b>Estimated Fleet Size/Number of Units: </b>
          <xsl:value-of select="estimatedFleetSize"/>
        </p>
        <p>
          <b>Message: </b>
        </p>
        <p>
          <xsl:value-of select="message"/>
        </p>       
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
