Imports System.ComponentModel

Public Class MainForm
    Private Sub MainForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Application.ExitThread()
    End Sub

    Private Sub MainForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Text = $"Welcome [{My.Application.UserName}]"
    End Sub
End Class