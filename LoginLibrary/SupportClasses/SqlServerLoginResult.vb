Namespace SupportClasses
    Public Class SqlServerLoginResult
        Public Property Success() As Boolean
        Public ReadOnly Property Failed() As Boolean
            Get
                Return Success = False
            End Get
        End Property
        Public Property GenericException() As Boolean
        Public Property Message() As String

        Public Overrides Function ToString() As String
            Return Message
        End Function
    End Class
End Namespace