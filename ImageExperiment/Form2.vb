Public Class Form2
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.mainDirectory <> "" Then
            textBox1.Text = My.Settings.mainDirectory
        End If
        If My.Settings.altDirectory <> "" Then
            textBox2.Text = My.Settings.altDirectory
        End If
        numericUpDown1.Value = My.Settings.speed
        numericUpDown3.Value = My.Settings.deviationInterval
        numericUpDown2.Value = My.Settings.transparency
        NumericUpDown4.Value = My.Settings.maxDeviation
        NumericUpDown5.Value = My.Settings.deviation
        checkBox1.Checked = My.Settings.brainwashMode
        checkBox1.Checked = My.Settings.brainwashMode
        controlsControl()
    End Sub
    Private Sub Form2_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If textBox1.Text <> ". . ." Then
            My.Settings.mainDirectory = textBox1.Text
        End If
        If textBox1.Text <> ". . ." Then
            My.Settings.altDirectory = textBox2.Text
        End If
        My.Settings.speed = Convert.ToInt32(numericUpDown1.Value)
        My.Settings.deviationInterval = Convert.ToInt32(numericUpDown3.Value)
        My.Settings.transparency = Convert.ToByte(numericUpDown2.Value)
        My.Settings.brainwashMode = checkBox1.Checked
        My.Settings.deviation = Convert.ToInt32(NumericUpDown5.Value)
        My.Settings.maxDeviation = NumericUpDown4.Value
        My.Settings.Save()
        Form1.Enabled = True
    End Sub

    Private Sub controlsControl()
        If (checkBox1.Checked Or My.Settings.brainwashMode = True) Then
            'Enable controls
            textBox2.Enabled = True
            label2.Enabled = True
            label3.Enabled = True
            Label7.Enabled = True
            Label8.Enabled = True
            numericUpDown3.Enabled = True
            NumericUpDown4.Enabled = True
            NumericUpDown5.Enabled = True
        Else
            'Disable controls
            textBox2.Enabled = False
            label2.Enabled = False
            label3.Enabled = False
            Label7.Enabled = False
            Label8.Enabled = False
            numericUpDown3.Enabled = False
            NumericUpDown4.Enabled = False
            NumericUpDown5.Enabled = False

        End If
    End Sub
    Private Sub checkBox1_CheckedChanged(sender As Object, e As EventArgs) Handles checkBox1.CheckedChanged
        My.Settings.brainwashMode = checkBox1.Checked
        controlsControl()
    End Sub

    Private Sub textBox1_Click(sender As Object, e As EventArgs) Handles textBox1.Click
        Dim dirBrowser As FolderBrowserDialog = New FolderBrowserDialog()
        dirBrowser.ShowDialog()
        If dirBrowser.SelectedPath <> "" Then
            My.Settings.mainDirectory = dirBrowser.SelectedPath
            textBox1.Text = dirBrowser.SelectedPath
        End If
    End Sub

    Private Sub textBox2_Click(sender As Object, e As EventArgs) Handles textBox2.Click
        Dim dirBrowser As FolderBrowserDialog = New FolderBrowserDialog()
        dirBrowser.ShowDialog()
        If dirBrowser.SelectedPath <> "" Then
            My.Settings.altDirectory = dirBrowser.SelectedPath
            textBox2.Text = dirBrowser.SelectedPath
        End If
    End Sub

    Private Sub NumericUpDown5_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown5.ValueChanged
        NumericUpDown4.Maximum = NumericUpDown5.Value
    End Sub
End Class