Public Class LoginForm
    Private userID As String = "spicyramen06"
    Private password As String = "96619"
    Public Shared attempts As Integer = 0
    Private Sub LoginButton_Click(sender As Object, e As EventArgs) Handles LoginButton.Click

        attempts += 1

        If attempts >= 4 Then
            LoginButton.Enabled = False
            MessageBox.Show("Attempts exceeded")
            Exit Sub
        End If
        If Not String.IsNullOrWhiteSpace(UserNameTextBox.Text) AndAlso Not String.IsNullOrWhiteSpace(PasswordTextBox.Text) Then
            If UserNameTextBox.Text = userID AndAlso PasswordTextBox.Text = password Then
                MessageBox.Show("Welcome")
            Else
                Controls.OfType(Of TextBox).ToList().ForEach(Sub(tb) tb.Text = "")
                MessageBox.Show("Invalid logn")
            End If
        End If
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Close()
    End Sub
End Class
