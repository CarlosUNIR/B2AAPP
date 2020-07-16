Imports System
Imports System.IdentityModel.Tokens
Imports System.ServiceModel.Security

Public Class FaceB2BCustomAlgorithmSuite
    Inherits SecurityAlgorithmSuite

    Public Overrides ReadOnly Property DefaultAsymmetricKeyWrapAlgorithm As String
        Get
            Return SecurityAlgorithms.RsaV15KeyWrap
        End Get
    End Property

    Public Overrides ReadOnly Property DefaultAsymmetricSignatureAlgorithm As String
        Get
            Return SecurityAlgorithms.RsaSha256Signature
        End Get
    End Property

    Public Overrides ReadOnly Property DefaultCanonicalizationAlgorithm As String
        Get
            Return SecurityAlgorithms.ExclusiveC14n
        End Get
    End Property

    Public Overrides ReadOnly Property DefaultDigestAlgorithm As String
        Get
            Return SecurityAlgorithms.Sha256Digest
        End Get
    End Property

    Public Overrides ReadOnly Property DefaultEncryptionAlgorithm As String
        Get
            Return SecurityAlgorithms.Aes128Encryption
        End Get
    End Property

    Public Overrides ReadOnly Property DefaultEncryptionKeyDerivationLength As Integer
        Get
            Return 128
        End Get
    End Property

    Public Overrides ReadOnly Property DefaultSignatureKeyDerivationLength As Integer
        Get
            Return 256
        End Get
    End Property

    Public Overrides ReadOnly Property DefaultSymmetricKeyLength As Integer
        Get
            Return 256
        End Get
    End Property

    Public Overrides ReadOnly Property DefaultSymmetricKeyWrapAlgorithm As String
        Get
            Return SecurityAlgorithms.Aes128KeyWrap
        End Get
    End Property

    Public Overrides ReadOnly Property DefaultSymmetricSignatureAlgorithm As String
        Get
            Return SecurityAlgorithms.HmacSha256Signature
        End Get
    End Property

    Public Overrides Function IsAsymmetricKeyLengthSupported(ByVal length As Integer) As Boolean
        Return length >= 1024 AndAlso length <= 4096
    End Function

    Public Overrides Function IsSymmetricKeyLengthSupported(ByVal length As Integer) As Boolean
        Return length >= 128 AndAlso length <= 256
    End Function
End Class
