'   FirmaXML 2.0
'   Ensamblaje.NET para permitir el firmado XMLDSig de un documento fiscal mexicano de formato XML.
'   DLL compilada con la versión de prueba de Visual Studio. PD. Comentario del autor original.

'   Ensamblaje.NET para firma XMLDSig y XAdES BES, utilizando el ensamblaje Microsoft.Xades,
'   OpenSource de Microsoft France.
Imports System
Imports System.IO
Imports System.Xml
Imports System.Runtime.InteropServices
Imports System.EnterpriseServices
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports System.Security.Cryptography.Xml
Imports Microsoft.Xades

Namespace JironAsociados.FirmaXML
    Interface IFirmaXML
        Function FirmarXDsig(ByVal XMLFILE As String, ByVal NSERIE As String) As String
        Function FirmarXAdes(ByVal XMLFILE As String, ByVal NSERIE As String) As String
    End Interface

    'Permitimos que esta clase pueda ser exportada como interfaz COM, para ser utilizada en 
    'entornos no .NET también
    '<Synchronization(SynchronizationOption.Required), JustInTimeActivation(True), ObjectPooling(Enabled:=True)>

    Public Class FirmaXML
        Inherits ServicedComponent
        'Implements IFirmaXML

        Public Sub New()
        End Sub

        <AutoComplete(True)>
        Public Function FirmarXDsig(ByVal XMLFILE As String, ByVal NSERIE As String) As String
            ' Método que recibe el texto de un documento XML y regresa ese mismo texto pero conteniendo 
            ' una Firma digital XMLDSig envolvente.

            ' Parámetro XMLFILE: Ubicación del documento a firmar.
            ' Parámetro NSERIE: Número de Serie del certificado a utilizar

            ' Obtenemos el certificado correspondiente al Número de serie dado
            Dim Certificado As X509Certificate2 = BuscarCertificado(NSERIE)
            ' Para utilizar un archivo pkcs de firmado digital
            'Dim Certificado As X509Certificate2 = New X509Certificate2("<Ubicación del archivo pkcs>", "<contraseña>")

            ' Obtenemos el objeto de llave privada del certificado
            Dim Llave As RSACryptoServiceProvider = TryCast(Certificado.PrivateKey, RSACryptoServiceProvider)

            ' Creamos el objeto firmante (Firma) asignándole el texto XML y la llave del certificado
            Dim Documento As XmlDocument = New XmlDocument()
            Documento.Load(XMLFILE)
            ' Documento.LoadXml(XML)

            Dim Firma As SignedXml = New SignedXml(Documento)
            Firma.SigningKey = Llave

            ' Creamos el nodo <Reference> con un subnodo <Transforms> conteniendo el elemento
            ' <Transform Algorithm = "http://www.w3.org/2000/09/xmldsig#enveloped-signature" />, y lo
            ' agregamos al objeto firmante
            Dim Referencia As Reference = New Reference()
            Referencia.Uri = ""
            Referencia.AddTransform(New XmlDsigEnvelopedSignatureTransform())
            Firma.AddReference(Referencia)

            ' Creamos el nodo <KeyInfo> con el subnodo <X509Data>, poniendo dentro de éste el 
            ' Certificado, su número de serie y la entidad emisora del mismo (primero estos dos últimos
            ' como subnodo < X509IssuerSerial >), y agregando todo al objeto firmante
            Dim NodoX509Data As KeyInfoX509Data = New KeyInfoX509Data()
            NodoX509Data.AddIssuerSerial(Certificado.Issuer, Certificado.GetSerialNumberString())
            NodoX509Data.AddCertificate(Certificado)
            Firma.KeyInfo = New KeyInfo()
            Firma.KeyInfo.AddClause(NodoX509Data)

            ' Generamos la firma digital y la agregamos al objeto Documento
            Firma.ComputeSignature()
            Documento.DocumentElement.AppendChild(Documento.ImportNode(Firma.GetXml(), True))

            ' Devolvemos el XML firmado
            Return Documento.OuterXml
        End Function

        <AutoComplete(True)>
        Public Function FirmarXAdes(ByVal XMLFILE As String, ByVal NSERIE As String) As String
            ' Método que recibe el texto de un documento XML y regresa ese mismo texto pero conteniendo 
            ' una Firma digital XAdES BES envolvente.

            ' Parámetro XMLFILE: Ubicación del documento a firmar.
            ' Parámetro NSERIE: Número de Serie del certificado a utilizar

            Dim resultado As String

            ' Obtenemos el certificado correspondiente al Número de serie dado
            Dim Certificado As X509Certificate2 = BuscarCertificado(NSERIE)
            ' Para utilizar un archivo pkcs de firmado digital
            ' X509Certificate2 Certificado = New X509Certificate2("<Ubicación del archivo pkcs>", "<contraseña>")

            ' Procedimiento para firmar con el objeto XadesSignedXml de Microsoft.Xades para firmar XML
            ' con firma envolvente
            Dim Documento As XmlDocument = New XmlDocument()
            Documento.Load(XMLFILE)

            Dim Firma As XadesSignedXml = New XadesSignedXml(Documento)
            Dim Llave As RSACryptoServiceProvider = TryCast(Certificado.PrivateKey, RSACryptoServiceProvider)
            Firma.SigningKey = Llave

            ' Agregamos referencias y objetos necesarios para la firma
            Dim Referencia As Reference = New Reference()
            Referencia.Uri = ""  ' Tomar ("digerir") todo el documento al crear la firma
            Referencia.AddTransform(New XmlDsigEnvelopedSignatureTransform())
            Firma.AddReference(Referencia)
            Dim xmlDsigC14NTransform As XmlDsigC14NTransform = New XmlDsigC14NTransform()
            Referencia.AddTransform(xmlDsigC14NTransform)
            Dim xmlDsigEnvelopedSignatureTransform As XmlDsigEnvelopedSignatureTransform = New XmlDsigEnvelopedSignatureTransform()
            Referencia.AddTransform(xmlDsigEnvelopedSignatureTransform)

            ' Creamos el nodo <KeyInfo> con el subnodo <X509Data>, poniendo dentro de éste el 
            ' Certificado, su número de serie y la entidad emisora del mismo (primero estos dos últimos
            ' como subnodo < X509IssuerSerial >), y agregando todo al objeto firmante
            Dim NodoX509Data As KeyInfoX509Data = New KeyInfoX509Data()
            NodoX509Data.AddIssuerSerial(Certificado.Issuer, Certificado.GetSerialNumberString())
            NodoX509Data.AddCertificate(Certificado)
            Firma.KeyInfo = New KeyInfo()
            Firma.KeyInfo.AddClause(NodoX509Data)

            ' Generamos la firma digital y la agregamos al objeto Documento
            Try
                Firma.ComputeSignature()
                Documento.DocumentElement.AppendChild(Documento.ImportNode(Firma.GetXml(), True))
                resultado = Documento.OuterXml
            Catch exception As Exception
                resultado = "Problema en la firma: " & exception.Message
            End Try

            ' Devolvemos el XML firmado o el error
            Return resultado
        End Function

        Protected Shared Function BuscarCertificado(ByVal NSERIE As String) As X509Certificate2
            ' Método para obtener el certificado que pertenece a un Número de Serie

            ' Cargamos la lista de certificados personales instalados en Windows
            Dim Certificados As X509Store = New X509Store(StoreName.My, StoreLocation.CurrentUser)
            Certificados.Open(OpenFlags.[ReadOnly])

            ' Buscamos el certificado del contribuyente
            For Each Resultado As X509Certificate2 In Certificados.Certificates
                If Resultado.GetSerialNumberString() = NSERIE Then Return Resultado
            Next

            Throw New Exception("No hay un certificado instalado para el Número de serie que se indicó.")
        End Function
    End Class
End Namespace
