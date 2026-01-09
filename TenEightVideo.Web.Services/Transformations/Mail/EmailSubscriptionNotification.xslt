<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="html" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="simpleValueInfo">
    <html xmlns="http://www.w3.org/1999/xhtml" >
      <head>
        <title>Email Subscription Notification</title>
      </head>
      <body>
        <p>
          <b>E-mail Address: </b>
          <xsl:element name="a">
            <xsl:attribute name="href">
              mailto:<xsl:value-of select="value"/>
            </xsl:attribute>
            <xsl:value-of select="value"/>
          </xsl:element>
        </p>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
