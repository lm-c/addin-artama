

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.IO;
using System.Diagnostics;
using LmCorbieUI;
using LmCorbieUI.LmForms;
using LmCorbieUI.Metodos;

namespace AddinCorbieArtama.VIEW
{
    public partial class FrmReportWorks : LmSingleForm
    {
        protected string FileNameCSV;

        public SldWorks swApp = new SldWorks();
        ModelDoc2 swModel = default(ModelDoc2);
        ModelDocExtension swModelDocExt;
        CustomPropertyManager swCustPropMgr;

        SortableBindingList<ReportWorks> _reports = new SortableBindingList<ReportWorks>();

        ReportWorks _report = new ReportWorks();

        public FrmReportWorks()
        {
            InitializeComponent();
        }

        private void FormatarGrid()
        {
            dgv.Grid.Columns["Nivel"].Width = 70;
            dgv.Grid.Columns["Qtd"].Width = 40;
            dgv.Grid.Columns["Codigo"].Width = 80;
            dgv.Grid.Columns["Componente"].Width = 80;
            dgv.Grid.Columns["Material"].Width = 100;
            dgv.Grid.Columns["Comprimento"].Width = 100;
            dgv.Grid.Columns["Seriado"].Width = 50;
            dgv.Grid.Columns["Operacao"].Width = 60;
            dgv.Grid.Columns["Maquina"].Width = 50;

            dgv.Grid.Columns["Denominacao"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgv.Grid.Columns["Qtd"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Grid.Columns["Qtd"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Grid.Columns["Codigo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Grid.Columns["Codigo"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Grid.Columns["Componente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Grid.Columns["Componente"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Grid.Columns["Material"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Grid.Columns["Material"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Grid.Columns["Comprimento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Grid.Columns["Comprimento"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Grid.Columns["Seriado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Grid.Columns["Seriado"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Grid.Columns["Operacao"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Grid.Columns["Operacao"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Grid.Columns["Maquina"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Grid.Columns["Maquina"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }

        private void FrmReportWorks_Load(object sender, EventArgs e)
        {
        }

        private void FrmReportWorks_Loaded(object sender, EventArgs e)
        {
            Invoke(new MethodInvoker(delegate ()
            {
                if (InfoSetting.UnicodeUtf8)
                    rdbUtf8.Checked = true;
                else if (InfoSetting.UnicodeIso)
                    rdbIso.Checked = true;

                this.txtSeriado.SelectedValueChanched -= this.TxtSeriado_SelectedValueChanched;
                txtSeriado.CarregarComboBoxEnum(typeof(SeriadoFiltro));
                this.txtSeriado.SelectedValueChanched += this.TxtSeriado_SelectedValueChanched;

                VerificarErros();
            }));
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            MsgBox.ShowWaitMessage("Lendo componentes da montagem...");

            try
            {
                if (swApp.ActiveDoc == null)
                {
                    MsgBox.Show($"Sem Documentos Abertos", "Addin LM Projetos",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                swModel = (ModelDoc2)swApp.ActiveDoc;

                if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
                {
                    swModel.ShowNamedView("*Isométrica");
                    swModel.ViewZoomtofit();

                    _reports = ReportWorksDATA.GetReport();
                    dgv.Grid.DataSource = _reports;

                    if (_reports != null)
                    {
                        FormatarGrid();
                        VerificarErros();
                    }
                }
                else
                {
                    MsgBox.Show($"Comando Apenas para Montagem", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show($"Erro ao carregar componentes\n\n{ex.Message}", "Addin LM Projetos",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                MsgBox.CloseWaitMessage();
            }
        }

        private void BtnAbrir_Click(object sender, EventArgs e)
        {
            if (swApp.ActiveDoc != null)
            {
                swModel = (ModelDoc2)swApp.ActiveDoc;
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = Path.GetDirectoryName(swModel.GetPathName());
                ofd.FileName = Path.GetFileNameWithoutExtension(swModel.GetPathName());
                ofd.Title = "Selecionar arquivo CSV";
                ofd.Filter = "CSV|*.csv|All files|*.*";
                ofd.DefaultExt = "csv";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _reports = null;
                    _reports = ReadFile(ofd.FileName);
                    dgv.Grid.DataSource = _reports;

                    FormatarGrid();

                    FileNameCSV = ofd.FileName;
                    VerificarErros();
                }
            }
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            if (_reports.Count == 0) return;
            if (swApp.ActiveDoc != null && !string.IsNullOrEmpty(((ReportWorks)_reports[0]).PathName))
            {
                FileNameCSV = Path.GetDirectoryName(((ReportWorks)_reports[0]).PathName) + "\\" + ((ReportWorks)_reports[0]).Componente + ".csv";

                WriteFile(FileNameCSV);

                if (File.Exists(FileNameCSV))
                    AbrirCSV(FileNameCSV);
            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog();
                if (!string.IsNullOrEmpty(FileNameCSV))
                    sfd.InitialDirectory = Path.GetDirectoryName(FileNameCSV);
                sfd.Title = "Salvar arquivo CSV do ReportWorks";
                sfd.Filter = "CSV|*.csv|All files|*.*";
                sfd.DefaultExt = "csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    WriteFile(sfd.FileName);

                    if (File.Exists(FileNameCSV))
                        AbrirCSV(FileNameCSV);
                }
            }
        }

        private bool Open3D()
        {
            bool _return = true;

            try
            {
                string openFileName = "";
                int status = 0;
                int warnings = 0;
                openFileName = ((ReportWorks)dgv.Grid.CurrentRow.DataBoundItem).PathName.ToUpper();

                if (openFileName.Contains("SLDPRT"))
                {
                    swApp.OpenDoc6(openFileName, (int)swDocumentTypes_e.swDocPART, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
                    int errors = 0;
                    swApp.ActivateDoc2(openFileName, false, (int)errors);
                }
                else if (openFileName.Contains("SLDASM"))
                {
                    swApp.OpenDoc6(openFileName, (int)swDocumentTypes_e.swDocASSEMBLY, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref status, ref warnings);
                    int errors = 0;
                    swApp.ActivateDoc2(openFileName, false, (int)errors);
                }
                _return = true;
            }
            catch (Exception)
            {
                _return = false;
            }

            return _return;
        }

        private void VerificarErros()
        {
            try
            {
                RetornarCorCelulas();

                int rowIndex = 0;

                foreach (ReportWorks rpt in _reports)
                {

                    rowIndex = _reports.IndexOf(rpt);

                    if (rowIndex == 0) continue;

                    if (((rpt.Componente.StartsWith("1") || rpt.Componente.StartsWith("2") || rpt.Componente.StartsWith("3")) && rpt.Seriado == "Não" && rpt.Interno != "Sim"))
                        AlterarCorCelulas(rowIndex, 3, 7);

                    if ((!rpt.Componente.StartsWith("1") && !rpt.Componente.StartsWith("2") && !rpt.Componente.StartsWith("3") && rpt.Seriado == "Sim"))
                        AlterarCorCelulas(rowIndex, 3, 7);

                    if ((rpt.Componente.StartsWith("1") || rpt.Componente.StartsWith("2") || rpt.Componente.StartsWith("3")) && rpt.Interno != "Sim" && string.IsNullOrEmpty(rpt.Operacao))
                        AlterarCorCelulas(rowIndex, 3, 8);

                    if ((!rpt.Componente.StartsWith("1") && !rpt.Componente.StartsWith("2") && !rpt.Componente.StartsWith("3") && rpt.Interno != "Sim") &&
                        (!string.IsNullOrEmpty(rpt.Componente) && (!string.IsNullOrEmpty(rpt.Operacao) || !string.IsNullOrEmpty(rpt.Comprimento))))
                        AlterarCorCelulas(rowIndex, 3, 8);

                    if (!string.IsNullOrEmpty(rpt.Codigo) && (string.IsNullOrEmpty(rpt.Comprimento)))
                        AlterarCorCelulas(rowIndex, 2, 6);

                    if (!string.IsNullOrEmpty(rpt.Codigo) && (string.IsNullOrEmpty(rpt.Operacao)))
                        AlterarCorCelulas(rowIndex, 2, 8);

                    if (string.IsNullOrEmpty(rpt.Codigo) && (string.IsNullOrEmpty(rpt.Componente)))
                        AlterarCorCelulas(rowIndex, 2, 3);

                    if (!string.IsNullOrEmpty(rpt.Componente) && (string.IsNullOrEmpty(rpt.Seriado)))
                        AlterarCorCelulas(rowIndex, 3, 7);

                    if (!string.IsNullOrEmpty(rpt.Componente) && (string.IsNullOrEmpty(rpt.Seriado)))
                        AlterarCorCelulas(rowIndex, 3, 7);

                    if (string.IsNullOrEmpty(rpt.Denominacao))
                        AlterarCorCelulas(rowIndex, 0, 5);
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show($"Não foi possível verificar erros\n\n{ex.Message}", "Addin LM Projetos",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Dgv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                try
                {
                    DeletearLinha();
                }
                catch (Exception ex)
                {
                    MsgBox.Show($"Erro ao deletar linha\n\n{ex.Message}", "Addin LM Projetos",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tsmOpen2D_Click(object sender, EventArgs e)
        {

        }

        private void TsmOpen3D_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Open3D()) return;
            }
            catch (Exception ex)
            {
                MsgBox.Show($"Erro ao abrir arquivo\n\n{ex.Message}", "Addin LM Projetos",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TsmDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DeletearLinha();
            }
            catch (Exception ex)
            {
                MsgBox.Show($"Erro ao deletar linha\n\n{ex.Message}", "Addin LM Projetos",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeletearLinha()
        {
            try
            {
                _reports.Remove((ReportWorks)dgv.Grid.CurrentRow.DataBoundItem);

                dgv.Grid.Refresh();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void TsmAdd_Click(object sender, EventArgs e)
        {
            try
            {
                _reports.Add(new ReportWorks());
                dgv.Grid.Refresh();
                dgv.Grid.Rows[dgv.Grid.RowCount - 1].Selected = true;
            }
            catch (Exception ex)
            {
                MsgBox.Show($"Erro ao adicionar linha\n\n{ex.Message}", "Addin LM Projetos",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TsmAltDados_Click(object sender, EventArgs e)
        {
            string fileName = null;
            try
            {
                if (Open3D())
                {
                    swModel = (ModelDoc2)swApp.ActiveDoc;
                    fileName = swModel.GetPathName().ToUpper();

                    swModelDocExt = swModel.Extension;
                    swCustPropMgr = swModelDocExt.get_CustomPropertyManager("");

                    if (!string.IsNullOrEmpty(((ReportWorks)dgv.Grid.CurrentRow.DataBoundItem).Codigo))
                    {
                        int nCutList = 1;
                        while (true)
                        {
                            int indexCurrent = _reports.IndexOf((ReportWorks)dgv.Grid.CurrentRow.DataBoundItem);
                            if (!string.IsNullOrEmpty(((ReportWorks)_reports[indexCurrent - nCutList]).Codigo))
                                nCutList++;
                            else
                                break;
                        }

                        if (!ReportWorksDATA.UpdateCutList(nCutList, (ReportWorks)dgv.Grid.CurrentRow.DataBoundItem))
                            MsgBox.Show($"Lista de Corte não foi atualizada", "Addin LM Projetos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (((ReportWorks)dgv.Grid.CurrentRow.DataBoundItem).Seriado == "Não")
                        {
                            swCustPropMgr.Add3("Denominação", (int)swCustomInfoType_e.swCustomInfoText,
                                ((ReportWorks)dgv.Grid.CurrentRow.DataBoundItem).Denominacao, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

                            swCustPropMgr.Add3("Componente", (int)swCustomInfoType_e.swCustomInfoText,
                                ((ReportWorks)dgv.Grid.CurrentRow.DataBoundItem).Componente, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
                        }

                        swCustPropMgr.Add3("Seriado", (int)swCustomInfoType_e.swCustomInfoText,
                            ((ReportWorks)dgv.Grid.CurrentRow.DataBoundItem).Seriado, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);

                        if (!string.IsNullOrEmpty(((ReportWorks)dgv.Grid.CurrentRow.DataBoundItem).Operacao))
                            swCustPropMgr.Add3("Operação", (int)swCustomInfoType_e.swCustomInfoText,
                             ((ReportWorks)dgv.Grid.CurrentRow.DataBoundItem).Operacao, (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show($"Erro ao salvar dados\n\n{ex.Message}", "Addin LM Projetos",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    swModel.Save();
                    swApp.CloseDoc(fileName);
                }
            }
        }

        private void AlterarCorCelulas(int rowIndex, int celIndex1, int celIndex2)
        {
            Color clr = Color.OrangeRed;
            foreach (DataGridViewCell Cell in dgv.Grid.Rows[rowIndex].Cells)
                if (Cell.ColumnIndex == 0 || Cell.ColumnIndex == celIndex1 || Cell.ColumnIndex == celIndex2)
                    Cell.Style.BackColor = Cell.Style.BackColor = clr;
        }

        private void RetornarCorCelulas()
        {
            foreach (DataGridViewRow row in dgv.Grid.Rows)
                foreach (DataGridViewCell Cell in row.Cells)
                    Cell.Style.BackColor = Color.White;
        }

        private SortableBindingList<ReportWorks> ReadFile(string filename)
        {
            Cursor = Cursors.WaitCursor;

            List<ReportWorks> listReport = new List<ReportWorks>();

            try
            {
                string encoding = rdbUtf8.Checked ? "UTF-8" : "ISO-8859-1";
                using (StreamReader leitor = new StreamReader(filename, Encoding.GetEncoding(encoding)))
                {
                    string linha = leitor.ReadLine();
                    linha = leitor.ReadLine();

                    while (linha != null)
                    {
                        string[] columns = linha.Split(';');

                        //int id = Convert.ToInt32(columns[0]);

                        listReport.Add(new ReportWorks()
                        {
                            Nivel = columns[0].Trim().Replace(",", "."),
                            Qtd = Convert.ToDouble(columns[1].Trim()).ToString("0"),
                            Codigo = columns[2].Trim(),
                            Componente = columns[3].Trim(),
                            Material = columns[4].Trim(),
                            Denominacao = columns[5].Trim(),
                            Comprimento = columns[6].Trim().Replace(",", "."),
                            Seriado = columns[7].Trim(),
                            Operacao = columns[8].Trim(),
                            Maquina = columns[9].Trim(),
                        });

                        linha = leitor.ReadLine();
                    }
                    leitor.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show($"Erro ao ler arquivo\n\n{ex.Message}", "Addin LM Projetos",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            return new SortableBindingList<ReportWorks>(listReport);
        }

        private bool WriteFile(string filename)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    string encoding = rdbUtf8.Checked ? "UTF-8" : "ISO-8859-1";
                    using (StreamWriter file = new StreamWriter(fs, Encoding.GetEncoding(encoding)))
                    {
                        string linha = "Nivel;Quantidade;Codigo;Componente;Material;Denominacao;Comprimento;Seriado;Operacao;Maquina";
                        file.WriteLine(linha);

                        foreach (DataGridViewRow row in dgv.Grid.Rows)
                        {
                            linha = string.Empty;
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (cell.ColumnIndex == 1)
                                    linha += $"{Convert.ToDouble(cell.Value).ToString("0.000000")};" ?? string.Empty + ";";
                                else
                                    linha += $"{Convert.ToString(cell.Value)};" ?? string.Empty + ";";
                            }
                            linha = linha.Substring(0, linha.Count() - 1);
                            file.WriteLine(linha);
                        }
                        file.Close();
                    }
                    fs.Close();
                    System.Threading.Thread.Sleep(500);
                }

                return true;
            }
            catch (Exception ex)
            {
                MsgBox.Show($"Erro ao escrever arquivo\n\n{ex.Message}", "Addin LM Projetos",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return false;
        }

        private void AbrirCSV(string FileNameCSV)
        {
            if (MsgBox.Show("ReportWorks Salvo Com Sucesso.\nDeseja Abrir o Arquivo", "Addin LM Projetos",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Process.Start(FileNameCSV);
            }
        }

        private void TxtSeriado_SelectedValueChanched(object sender, EventArgs e)
        {
            try
            {
                if (txtSeriado.SelectedValue == null)
                    txtSeriado.SelectedValue = SeriadoFiltro.Todos;

                if (dgv.Grid.DataSource == null)
                    return;

                CurrencyManager cm = (CurrencyManager)BindingContext[dgv.Grid.DataSource];
                cm.EndCurrentEdit();
                cm.ResumeBinding();
                cm.SuspendBinding();

                foreach (DataGridViewRow row in dgv.Grid.Rows)
                {
                    string celValue = Convert.ToString(row.Cells["Seriado"].Value);

                    if ((SeriadoFiltro)txtSeriado.SelectedValue == SeriadoFiltro.Sim)
                    {
                        if (celValue == "Sim")
                            row.Visible = true;
                        else
                            row.Visible = false;
                    }
                    else if ((SeriadoFiltro)txtSeriado.SelectedValue == SeriadoFiltro.Nao)
                    {
                        if (celValue == "Não")
                            row.Visible = true;
                        else
                            row.Visible = false;
                    }
                    else if ((SeriadoFiltro)txtSeriado.SelectedValue == SeriadoFiltro.Vazia)
                    {
                        if (string.IsNullOrEmpty(celValue))
                            row.Visible = true;
                        else
                            row.Visible = false;
                    }
                    else if ((SeriadoFiltro)txtSeriado.SelectedValue == SeriadoFiltro.Todos)
                    {
                        row.Visible = true;
                    }
                }

                dgv.Grid.Refresh();
                dgv.Grid.Update();
            }
            catch (Exception ex)
            {
                MsgBox.Show($"Erro ao filtrar Seriado\n\n{ex.Message}", "Addin LM Projetos",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSubstituir_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtDe.Text) || string.IsNullOrEmpty(txtPara.Text)) return;

                string de = txtDe.Text;
                string para = txtPara.Text;

                foreach (var report in _reports.Where(x=> !string.IsNullOrEmpty(x.Operacao) ).ToList())
                {
                    if (report.Operacao.Contains(de))
                        report.Operacao = report.Operacao.Replace(de, para);
                }

                dgv.Grid.Refresh();

                MsgBox.Show("Substituido com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MsgBox.Show($"Erro ao substituir componentes\n\n{ex.Message}", "Addin LM Projetos",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RdbUtf8_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbUtf8.Checked)
            {
                InfoSetting.UnicodeUtf8 = true;
                InfoSetting.UnicodeIso = false;
                InfoSetting.Salvar();
            }
        }

        private void RdbIso_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbIso.Checked)
            {
                InfoSetting.UnicodeUtf8 = false;
                InfoSetting.UnicodeIso = true;
                InfoSetting.Salvar();
            }
        }

        private void Dgv_ProcurarTextChanged(object sender, EventArgs e)
        {
            dgv.CarregarGrid(_reports);
        }
    }
}
