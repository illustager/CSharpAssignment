namespace e3;

public class Form1 : Form
{
    TextBox txtFile1 = new();
    TextBox txtFile2 = new();
    Button btnBrowse1 = new();
    Button btnBrowse2 = new();
    Button btnMerge = new();
    Label lblStatus = new();

    public Form1()
    {
        Text = "File Merge Tool";
        Width = 600;
        Height = 250;
        StartPosition = FormStartPosition.CenterScreen;

        var lbl1 = new Label { Text = "File 1:", Location = new Point(20, 20), Width = 50 };
        txtFile1.Location = new Point(80, 18);
        txtFile1.Width = 380;
        btnBrowse1.Text = "Browse";
        btnBrowse1.Location = new Point(470, 17);
        btnBrowse1.Width = 80;
        btnBrowse1.Click += (_, _) =>
        {
            using var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
                txtFile1.Text = dlg.FileName;
        };

        var lbl2 = new Label { Text = "File 2:", Location = new Point(20, 60), Width = 50 };
        txtFile2.Location = new Point(80, 58);
        txtFile2.Width = 380;
        btnBrowse2.Text = "Browse";
        btnBrowse2.Location = new Point(470, 57);
        btnBrowse2.Width = 80;
        btnBrowse2.Click += (_, _) =>
        {
            using var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
                txtFile2.Text = dlg.FileName;
        };

        btnMerge.Text = "Merge";
        btnMerge.Location = new Point(200, 100);
        btnMerge.Width = 150;
        btnMerge.Click += BtnMerge_Click;

        lblStatus.Text = "Ready.";
        lblStatus.Location = new Point(20, 150);
        lblStatus.Width = 540;

        Controls.AddRange(new Control[] {
            lbl1, txtFile1, btnBrowse1,
            lbl2, txtFile2, btnBrowse2,
            btnMerge, lblStatus
        });
    }

    void BtnMerge_Click(object? sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtFile1.Text) || string.IsNullOrWhiteSpace(txtFile2.Text))
            {
                lblStatus.Text = "Error: Please select both files.";
                return;
            }

            var content1 = File.ReadAllText(txtFile1.Text);
            var content2 = File.ReadAllText(txtFile2.Text);
            var merged = content1 + Environment.NewLine + content2;

            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            Directory.CreateDirectory(dir);

            var outputPath = Path.Combine(dir, "merged.txt");
            File.WriteAllText(outputPath, merged);

            lblStatus.Text = $"Merged to: {outputPath}";
        }
        catch (Exception ex)
        {
            lblStatus.Text = $"Error: {ex.Message}";
        }
    }
}

internal class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }
}
