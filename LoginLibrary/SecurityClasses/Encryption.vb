Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Namespace SecurityClasses
    ''' <summary>
    ''' Simple encryption decryption of strings
    ''' </summary>
    Public Class Encryption
        Public Function Encrypt(plainText As String, secretKey As String) As Byte()
            Dim encryptedPassword As Byte()
            Using outputStream = New MemoryStream()
                Dim algorithm As RijndaelManaged = getAlgorithm(secretKey)
                Using cryptoStream =
                    New CryptoStream(outputStream, algorithm.CreateEncryptor(),
                                     CryptoStreamMode.Write)
                    Dim inputBuffer() As Byte = Encoding.Unicode.GetBytes(plainText)
                    cryptoStream.Write(inputBuffer, 0, inputBuffer.Length)
                    cryptoStream.FlushFinalBlock()
                    encryptedPassword = outputStream.ToArray()
                End Using
            End Using
            Return encryptedPassword
        End Function

        Public Function Decrypt(encryptedBytes As Byte(), secretKey As String) As String
            Dim plainText As String = Nothing
            Using inputStream = New MemoryStream(encryptedBytes)
                Dim algorithm As RijndaelManaged = getAlgorithm(secretKey)
                Using cryptoStream =
                    New CryptoStream(inputStream, algorithm.CreateDecryptor(),
                                     CryptoStreamMode.Read)
                    Dim outputBuffer(0 To CType(inputStream.Length - 1, Integer)) As Byte
                    Dim readBytes As Integer =
                            cryptoStream.Read(outputBuffer, 0, CType(inputStream.Length, Integer))
                    plainText = Encoding.Unicode.GetString(outputBuffer, 0, readBytes)
                End Using
            End Using
            Return plainText
        End Function
        Private Function getAlgorithm(secretKey As String) As RijndaelManaged
            Const salt As String = "akl~jdf"
            Const keySize As Integer = 256

            Dim keyBuilder = New Rfc2898DeriveBytes(secretKey, Encoding.Unicode.GetBytes(salt))
            Dim algorithm = New RijndaelManaged()
            algorithm.KeySize = keySize
            algorithm.IV = keyBuilder.GetBytes(CType(algorithm.BlockSize / 8, Integer))
            algorithm.Key = keyBuilder.GetBytes(CType(algorithm.KeySize / 8, Integer))
            algorithm.Padding = PaddingMode.PKCS7
            Return algorithm
        End Function
    End Class
End Namespace