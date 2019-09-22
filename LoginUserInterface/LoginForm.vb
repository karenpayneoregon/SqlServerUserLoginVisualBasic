Imports LoginLibrary.DataClasses
Imports LoginLibrary.SecurityClasses

Public Class LoginForm
    ''' <summary>
    ''' Toggle visibility of password
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ShowHidePasswordCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ShowHidePasswordCheckBox.CheckedChanged
        If ShowHidePasswordCheckBox.Checked Then
            PasswordTextBox.PasswordChar = "*"c
        Else
            PasswordTextBox.PasswordChar = ControlChars.NullChar
        End If
    End Sub
    ''' <summary>
    ''' Perform login
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LoginButton_Click(sender As Object, e As EventArgs) Handles LoginButton.Click

        If Not String.IsNullOrWhiteSpace(UserNameTextBox.Text) AndAlso Not String.IsNullOrWhiteSpace(PasswordTextBox.Text) Then

            Dim ops = New DatabaseUser("KARENS-PC", "UserLoginExample")
            Dim tester = New Encryption

            ' encrypt user name and password
            Dim userNameBytes = tester.Encrypt(UserNameTextBox.Text, "111")
            Dim passwordBytes = tester.Encrypt(PasswordTextBox.Text, "111")

            Dim results = ops.Login(userNameBytes, passwordBytes)

            '
            ' Login recognized (does not know if the user has proper permissions to the tables at this point)
            '
            If results.Success Then
                Hide()
                Dim mainForm As New MainForm(userNameBytes, passwordBytes)
                mainForm.ShowDialog()
            Else
                MessageBox.Show(results.Message)
            End If
        Else
            MessageBox.Show("Incomplete information to continue.")
        End If
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Close()
    End Sub
End Class
