using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DecisionMaster.AlgorithmsLibrary.Algorithms.PROMETHEE;
using DecisionMaster.AlgorithmsLibrary.Algorithms.SMART;
using DecisionMaster.AlgorithmsLibrary.Algorithms.REGIME;
using DecisionMaster.AlgorithmsLibrary.Algorithms.ELECTRE;
using DecisionMaster.AlgorithmsLibrary.Interfaces;
using DecisionMaster.AlgorithmsLibrary.Models;
using DecisionMaster.UserInterface.Controllers;

namespace DecisionMaster.UserInterface
{
   
    public partial class FormDecisionMaster : Form
    {
        SolutionController controller = new SolutionController();
        private int EditingIndex = -1;

        public static Dictionary<String, double> QualitativeToDouble = new Dictionary<String, double>
        {
            {"Poor", 1 },
            {"Fairly Weak", 2 },
            {"Medium", 3 },
            {"Fairly Good", 4 },
            {"Good", 5 },
            {"Very Good", 6 },
            {"Excellent", 7 }
        };

        public FormDecisionMaster()
        {
            InitializeComponent();
        }

        private void dataGridViewCriteriasData_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
           
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("OK");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tabControlMain.Controls[2].Enabled = false;
            tabControlMain.Controls[1].Enabled = false;
            EditingIndex = -1;

            dataGridViewCriteriasData.Enabled = false;
            toolStripCriteriaManager.Enabled = false;
            panelNewCriteriaData.Enabled = true;

            comboBoxCriteriaType.Enabled = true;
            textBoxCriteriaTitle.Enabled = true;

            comboBoxCriteriaDitection.Enabled = false;

            textBoxQuantitativeMinValue.Enabled = false;
            textBoxQuantitativeMaxValue.Enabled = false;
            comboBoxQualitativeMaxValue.Enabled = false;
            comboBoxQualitativeMinValue.Enabled = false;

            comboBoxSpecifyPROMETHEE.Enabled = false;
            comboBoxPreferenceFunction.Enabled = false;
            textBoxPreferenceParameter1.Visible = false;
            labelPreferenceParameter1.Visible = false;
            textBoxPreferenceParameter2.Visible = false;
            labelPreferenceParameter2.Visible = false;

            buttonApplyCriteria.Enabled = false;
            buttonCriteriaCancel.Enabled = true;
        }

        private void comboBoxCriteriaType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectedIndex = comboBoxCriteriaType.SelectedIndex;
            if (SelectedIndex >= 0)
            {
                comboBoxCriteriaDitection.Enabled = true;
                if (SelectedIndex == 0)
                {
                    textBoxQuantitativeMaxValue.Visible = true;
                    textBoxQuantitativeMinValue.Visible = true;
                    comboBoxQualitativeMaxValue.Visible = false;
                    comboBoxQualitativeMinValue.Visible = false;
                }
                else
                {
                    textBoxQuantitativeMaxValue.Visible = false;
                    textBoxQuantitativeMinValue.Visible = false;
                    comboBoxQualitativeMaxValue.Visible = true;
                    comboBoxQualitativeMinValue.Visible = true;
                }
                if (comboBoxCriteriaDitection.SelectedIndex >= 0)
                {
                    comboBoxCriteriaDitection_SelectedIndexChanged(sender, e);
                }
            }
            else
            {
                comboBoxCriteriaDitection.Enabled = false;
                textBoxQuantitativeMinValue.Enabled = false;
                textBoxQuantitativeMaxValue.Enabled = false;
                comboBoxQualitativeMaxValue.Enabled = false;
                comboBoxQualitativeMinValue.Enabled = false;

                comboBoxSpecifyPROMETHEE.Enabled = false;
                comboBoxPreferenceFunction.Enabled = false;
                textBoxPreferenceParameter1.Visible = false;
                labelPreferenceParameter1.Visible = false;
                textBoxPreferenceParameter2.Visible = false;
                labelPreferenceParameter2.Visible = false;

                buttonApplyCriteria.Enabled = false;
                buttonCriteriaCancel.Enabled = true;
            }
        }

        private void comboBoxCriteriaDitection_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectedIndex = comboBoxCriteriaDitection.SelectedIndex;
            if (SelectedIndex >= 0)
            {
                comboBoxSpecifyPROMETHEE.Enabled = true;
                comboBoxSpecifyPROMETHEE.SelectedIndex = 0;

                comboBoxSpecifyELECTRE.Enabled = true;
                comboBoxSpecifyELECTRE.SelectedIndex = 0;

                buttonApplyCriteria.Enabled = true;

                if (comboBoxCriteriaType.SelectedIndex == 0)
                {
                    textBoxQuantitativeMaxValue.Enabled = true;
                    textBoxQuantitativeMinValue.Enabled = true;
                }
                else
                {
                    comboBoxQualitativeMaxValue.Enabled = true;
                    comboBoxQualitativeMinValue.Enabled = true;
                }
            }
            else
            {
                textBoxQuantitativeMinValue.Enabled = false;
                textBoxQuantitativeMaxValue.Enabled = false;
                comboBoxQualitativeMaxValue.Enabled = false;
                comboBoxQualitativeMinValue.Enabled = false;
                comboBoxSpecifyPROMETHEE.Enabled = false;
                comboBoxPreferenceFunction.Enabled = false;
                textBoxPreferenceParameter1.Visible = false;
                labelPreferenceParameter1.Visible = false;
                textBoxPreferenceParameter2.Visible = false;
                labelPreferenceParameter2.Visible = false;

                buttonApplyCriteria.Enabled = false;
                buttonCriteriaCancel.Enabled = true;
            }
        }

        private void comboBoxSpecifyPROMETHEE_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectedIndex = comboBoxSpecifyPROMETHEE.SelectedIndex;
            comboBoxPreferenceFunction.Enabled = (SelectedIndex == 2);
        }

        private void comboBoxPreferenceFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectedIndex = comboBoxPreferenceFunction.SelectedIndex;
            switch (SelectedIndex)
            {
                case 0:
                    labelPreferenceParameter1.Visible = false;
                    textBoxPreferenceParameter1.Visible = false;
                    labelPreferenceParameter2.Visible = false;
                    textBoxPreferenceParameter2.Visible = false;
                    break;
                case 1:
                    labelPreferenceParameter1.Visible = true;
                    textBoxPreferenceParameter1.Visible = true;
                    labelPreferenceParameter2.Visible = false;
                    textBoxPreferenceParameter2.Visible = false;

                    labelPreferenceParameter1.Text = "l";
                    break;
                case 2:
                    labelPreferenceParameter1.Visible = true;
                    textBoxPreferenceParameter1.Visible = true;
                    labelPreferenceParameter2.Visible = false;
                    textBoxPreferenceParameter2.Visible = false;

                    labelPreferenceParameter1.Text = "m";
                    break;
                case 3:
                    labelPreferenceParameter1.Visible = true;
                    textBoxPreferenceParameter1.Visible = true;
                    labelPreferenceParameter2.Visible = true;
                    textBoxPreferenceParameter2.Visible = true;

                    labelPreferenceParameter1.Text = "q";
                    labelPreferenceParameter2.Text = "p";
                    break;
                case 4:
                    labelPreferenceParameter1.Visible = true;
                    textBoxPreferenceParameter1.Visible = true;
                    labelPreferenceParameter2.Visible = true;
                    textBoxPreferenceParameter2.Visible = true;

                    labelPreferenceParameter1.Text = "s";
                    labelPreferenceParameter2.Text = "r";
                    break;
                case 5:
                    labelPreferenceParameter1.Visible = true;
                    textBoxPreferenceParameter1.Visible = true;
                    labelPreferenceParameter2.Visible = false;
                    textBoxPreferenceParameter2.Visible = false;

                    labelPreferenceParameter1.Text = "sigma";
                    break;
            }
        }

        private void ClearCriteriaPanel()
        {
            textBoxPreferenceParameter1.Text = "";
            textBoxPreferenceParameter2.Text = "";
            textBoxQuantitativeMaxValue.Text = "";
            textBoxQuantitativeMinValue.Text = "";
            textBoxCriteriaWeight.Text = "";
            textBoxCriteriaTitle.Text = "";

            comboBoxCriteriaType.SelectedIndex = -1;
            comboBoxCriteriaDitection.SelectedIndex = -1;

            comboBoxQualitativeMaxValue.SelectedIndex = -1;
            comboBoxQualitativeMinValue.SelectedIndex = -1;

            comboBoxSpecifyPROMETHEE.SelectedIndex = -1;
            comboBoxPreferenceFunction.SelectedIndex = -1;

            comboBoxSpecifyELECTRE.SelectedIndex = -1;            

            panelNewCriteriaData.Enabled = false;
            toolStripCriteriaManager.Enabled = true;
            dataGridViewCriteriasData.Enabled = true;
        }

        private void buttonCriteriaCancel_Click(object sender, EventArgs e)
        {
            ClearCriteriaPanel();
            tabControlMain.Controls[1].Enabled = true;
            tabControlMain.Controls[2].Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClearCriteriaPanel();
        }

        private double [] CheckAndGetQuantitativeMinMax()
        {
            double[] result = new double[2];
            if (textBoxQuantitativeMaxValue.Text == "" || !double.TryParse(textBoxQuantitativeMaxValue.Text, out result[1]))
            {
                throw new Exception("Criteria's max value is invalid");
            }
            else if (textBoxQuantitativeMaxValue.Text == "" || !double.TryParse(textBoxQuantitativeMinValue.Text.ToString(), out result[0]))
            {
                throw new Exception("Criteria's min value is invalid");
            } 
            else if (result[0] > result[1])
            {
                throw new Exception("Min value must be less than Max value");
            }
            return result;
        }

        private double[] CheckAndGetQualitativeMinMax()
        {
            if (comboBoxQualitativeMaxValue.SelectedIndex == -1)
            {
                throw new Exception("Criteria's max value is empty");
            }
            else if (comboBoxQualitativeMinValue.SelectedIndex == -1)
            {
                throw new Exception("Criteria's max value is empty");
            }
            else if (comboBoxQualitativeMinValue.SelectedIndex > comboBoxQualitativeMaxValue.SelectedIndex)
            {
                throw new Exception("Min value must be less than Max value");
            }
            double[] result =
            {
                (double)(comboBoxQualitativeMinValue.SelectedIndex + 1),
                (double)(comboBoxQualitativeMaxValue.SelectedIndex + 1)
            };
            return result;
        }


        private double [] CheckAndGetMinMaxValues()
        {
            return (comboBoxCriteriaType.SelectedIndex == 0 ?
                CheckAndGetQuantitativeMinMax() :
                CheckAndGetQualitativeMinMax()
                );
        }

        private ELECTREParameters CheckAndGetELECTREValues()
        {
            ELECTREParameters result = new ELECTREParameters();

            double p=0, v=0, q=0;
            if (textBoxElectreP.Text == "" || double.TryParse(textBoxElectreP.Text, out p) == false)
            {
                throw new Exception("ELECTRE parameter P is empty");
            }
            if (textBoxElectreQ.Text == "" || double.TryParse(textBoxElectreQ.Text, out q) == false)
            {
                throw new Exception("ELECTRE parameter Q is empty");
            }
            if (textBoxElectreV.Text == "" || double.TryParse(textBoxElectreV.Text, out v) == false)
            {
                throw new Exception("ELECTRE parameter V is empty");
            }

            if (p < q)
            {
                throw new Exception("ELECTRE's data is invalid: P must be greater or equal Q");
            }
            if (v < q)
            {
                throw new Exception("ELECTRE's data is invalid: V must be greater or equal Q");
            }

            result.p = p;
            result.q = q;
            result.v = v;
            return result;
        }

        private PreferenceFunction CheckAndGetPROMETHEEValues()
        {
            PreferenceFunction result = null;
            if (comboBoxPreferenceFunction.SelectedIndex == -1)
            {
                throw new Exception("Preference function must be selected");
            }
            else
            {
                int SelectedIndex = comboBoxPreferenceFunction.SelectedIndex;
                double LParam, RParam;
                if (SelectedIndex == 0)
                {
                    return new PreferenceFunction(PreferenceFunctionEnum.UsualCriterion);
                }
                if (SelectedIndex == 1 || SelectedIndex == 2 || SelectedIndex == 5)
                {
                    if (double.TryParse(textBoxPreferenceParameter1.Text, out LParam) == false)
                    {
                        throw new Exception(comboBoxPreferenceFunction.SelectedItem.ToString() + " parameter is invalid");
                    }
                    else
                    {
                        result =  new PreferenceFunction(
                            PreferenceFunctionEnum.QuasiCriterion,
                            new List<double> { LParam }
                            );
                    }
                }
                else if (SelectedIndex == 3 || SelectedIndex == 4)
                {
                    if (double.TryParse(textBoxPreferenceParameter1.Text, out LParam) == false ||
                        double.TryParse(textBoxPreferenceParameter2.Text, out RParam) == false)
                    {
                        throw new Exception(comboBoxPreferenceFunction.SelectedItem.ToString() + " parameters are invalid");
                    }
                    else
                    {
                        result = new PreferenceFunction(
                            PreferenceFunctionEnum.LevelCriterion,
                            new List<double> { LParam, RParam }
                            );
                    }
                }                
            }
            return result;
        }

        private void CheckAndCreateCriteriaData(CriteriaController data)
        {
            if (textBoxCriteriaTitle.Text == "")
            {
                throw new Exception("Criteria's title is empty");
            }
            double weight;
            if (textBoxCriteriaWeight.Text == "" || !double.TryParse(textBoxCriteriaWeight.Text, out weight))
            {
                throw new Exception("Criteria's weight is empty or invalid");
            }
            data.Weight = weight;
            data.Title = textBoxCriteriaTitle.Text;
            data.CriteriaType = (CriteriaTypeEnum)comboBoxCriteriaType.SelectedIndex;
            data.CriteriaDirection = (CriteriaDirectionType)(comboBoxCriteriaDitection.SelectedIndex + 1);


            double[] MinMaxValues = CheckAndGetMinMaxValues();
            data.MinValue = MinMaxValues[0];
            data.MaxValue = MinMaxValues[1];

            data.PROMETHEEConfiguration = (SpecialParametersEnum)comboBoxSpecifyPROMETHEE.SelectedIndex;
            if (comboBoxSpecifyPROMETHEE.SelectedIndex == 2)
            {
                data.PreferenceFunction = CheckAndGetPROMETHEEValues();
            }
            if (data.PROMETHEEConfiguration == SpecialParametersEnum.Default)
            {
                data.PreferenceFunction = new PreferenceFunction(PreferenceFunctionEnum.UsualCriterion);
            }

            data.ELECTREConfiguration = (SpecialParametersEnum)comboBoxSpecifyELECTRE.SelectedIndex;
            if (comboBoxSpecifyELECTRE.SelectedIndex == 2)
            {
                data.ELECTREspecialParameters = CheckAndGetELECTREValues();
            }
            if (comboBoxSpecifyELECTRE.SelectedIndex == 1)
            {
                data.ELECTREspecialParameters = new ELECTREParameters
                {
                    p = 0,
                    q = 0,
                    v = 0
                };
            }

        }

        private void AddToCriteriaDataGrid(CriteriaController data)
        {

            DataGridViewRow NewRow = null;
            if (EditingIndex == -1)
            {
                NewRow = new DataGridViewRow();
                while (NewRow.Cells.Count < dataGridViewCriteriasData.Columns.Count)
                {
                    NewRow.Cells.Add(new DataGridViewTextBoxCell());
                }
            }
            else
            {
                NewRow = dataGridViewCriteriasData.Rows[EditingIndex];
            }
            NewRow.Cells[0].Value = (EditingIndex == -1 ? controller._criterias.Criterias.Count : EditingIndex);
            NewRow.Cells[1].Value = data.Title;
            NewRow.Cells[2].Value = data.Weight;
            NewRow.Cells[3].Value = data.CriteriaType.ToString();
            NewRow.Cells[4].Value = data.CriteriaDirection.ToString();
            NewRow.Cells[5].Value = data.MinValue;
            NewRow.Cells[6].Value = data.MaxValue;
            NewRow.Cells[7].Value = data.PROMETHEEConfiguration.ToString();
            NewRow.Cells[8].Value = data.ELECTREConfiguration.ToString();
            if (EditingIndex == -1)
            {
                dataGridViewCriteriasData.Rows.Add(NewRow);
            }
        }

        void AddCriteriaColumn(CriteriaController data)
        {
            if (data.CriteriaType == CriteriaTypeEnum.Qualitative)
            {
                DataGridViewComboBoxColumn NewCol = new DataGridViewComboBoxColumn();
                NewCol.Name = data.Title;
                NewCol.HeaderText = data.Title;

                NewCol.Items.Add("Poor");
                NewCol.Items.Add("Fairly Weak");
                NewCol.Items.Add("Medium");
                NewCol.Items.Add("Fairly Good");
                NewCol.Items.Add("Good");
                NewCol.Items.Add("Very Good");
                NewCol.Items.Add("Excellent");


                if (EditingIndex == -1)
                {
                    dataGridViewAlternatives.Columns.Add(NewCol);
                }
                else
                {                    
                    dataGridViewAlternatives.Columns.Insert(EditingIndex + 1, NewCol);
                    dataGridViewAlternatives.Columns.RemoveAt(EditingIndex + 2);
                }
            }
            else
            {
                DataGridViewTextBoxColumn NewCol = new DataGridViewTextBoxColumn();
                NewCol.Name = data.Title;
                NewCol.HeaderText = data.Title;


                if (EditingIndex == -1)
                {
                    dataGridViewAlternatives.Columns.Add(NewCol);
                }
                else
                {
                    dataGridViewAlternatives.Columns.Insert(EditingIndex + 1, NewCol);
                    dataGridViewAlternatives.Columns.RemoveAt(EditingIndex + 2);
                }
            }
        }

        private void buttonApplyCriteria_Click(object sender, EventArgs e)
        {
            CriteriaController data = new CriteriaController();
            try
            {
                CheckAndCreateCriteriaData(data);
                AddToCriteriaDataGrid(data);
                if (EditingIndex == -1)
                {
                    controller._criterias.Criterias.Add(data);
                }
                else
                {
                    controller._criterias.Criterias[EditingIndex] = data;
                }
                ClearCriteriaPanel();
                AddCriteriaColumn(data);
                tabControlMain.Controls[1].Enabled = true;
                tabControlMain.Controls[2].Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillCriteriaPanel(CriteriaController data, object sender, EventArgs e)
        {
            panelNewCriteriaData.Enabled = true;
            textBoxCriteriaTitle.Enabled = true;
            textBoxCriteriaTitle.Text = data.Title;

            textBoxCriteriaWeight.Enabled = true;
            textBoxCriteriaWeight.Text = data.Weight.ToString();

            comboBoxCriteriaType.Enabled = true;
            comboBoxCriteriaType.SelectedIndex = (int)data.CriteriaType;

            comboBoxCriteriaDitection.Enabled = true;
            comboBoxCriteriaDitection.SelectedIndex = (int)data.CriteriaDirection-1;

            if (data.CriteriaType == CriteriaTypeEnum.Quantitative)
            {
                comboBoxQualitativeMaxValue.Visible = false;
                textBoxQuantitativeMaxValue.Visible = true;
                textBoxQuantitativeMaxValue.Enabled = true;
                textBoxQuantitativeMaxValue.Text = data.MaxValue.ToString();

                comboBoxQualitativeMinValue.Visible = false;
                textBoxQuantitativeMinValue.Visible = true;
                textBoxQuantitativeMinValue.Enabled = true;
                textBoxQuantitativeMinValue.Text = data.MinValue.ToString();
            }
            else
            {
                textBoxQuantitativeMaxValue.Visible = false;
                comboBoxQualitativeMaxValue.Visible = true;
                comboBoxQualitativeMaxValue.Enabled = true;
                comboBoxQualitativeMaxValue.SelectedIndex = (int)data.MaxValue-1;

                textBoxQuantitativeMinValue.Visible = false;
                comboBoxQualitativeMinValue.Visible = true;
                comboBoxQualitativeMinValue.Enabled = true;
                comboBoxQualitativeMinValue.SelectedIndex = (int)data.MinValue-1;
            }

            comboBoxSpecifyPROMETHEE.Enabled = true;
            comboBoxSpecifyPROMETHEE.SelectedIndex = (int)data.PROMETHEEConfiguration;

            comboBoxSpecifyPROMETHEE.SelectedIndex = (int)data.PROMETHEEConfiguration;
            if (data.PROMETHEEConfiguration == SpecialParametersEnum.Manual)
            {
                comboBoxPreferenceFunction.Visible = true;
                comboBoxPreferenceFunction.Enabled = true;
                comboBoxPreferenceFunction.SelectedIndex = (int)data.PreferenceFunction._type;
                comboBoxPreferenceFunction_SelectedIndexChanged(sender, e);
                if (data.PreferenceFunction._parameters.Count > 0)
                {
                    textBoxPreferenceParameter1.Text = data.PreferenceFunction._parameters[0].ToString();
                }
                if (data.PreferenceFunction._parameters.Count > 1)
                {
                    textBoxPreferenceParameter2.Text = data.PreferenceFunction._parameters[1].ToString();
                }
            }

            comboBoxSpecifyELECTRE.SelectedIndex = (int)data.ELECTREConfiguration;
            if (data.PROMETHEEConfiguration == SpecialParametersEnum.Manual)
            {
                textBoxElectreP.Enabled = true;
                textBoxElectreP.Text = data.ELECTREspecialParameters.p.ToString();
                textBoxElectreQ.Enabled = true;
                textBoxElectreQ.Text = data.ELECTREspecialParameters.q.ToString();
                textBoxElectreV.Enabled = true;
                textBoxElectreV.Text = data.ELECTREspecialParameters.v.ToString();
            }
            else
            {
                textBoxElectreP.Enabled = false;
                textBoxElectreQ.Enabled = false;
                textBoxElectreV.Enabled = false;
            }
        }

        private void toolStripButtonEditCriteria_Click(object sender, EventArgs e)
        {
            var SelectedRow = dataGridViewCriteriasData.SelectedRows;

            if (SelectedRow.Count > 0)
            {
                EditingIndex = int.Parse(SelectedRow[0].Cells[0].Value.ToString());
                FillCriteriaPanel(controller._criterias.Criterias[EditingIndex], sender, e);
                tabControlMain.Controls[1].Enabled = false;
                tabControlMain.Controls[2].Enabled = false;
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            var SelectedRow = dataGridViewCriteriasData.SelectedRows;
            if (SelectedRow.Count > 0)
            {
                EditingIndex = int.Parse(SelectedRow[0].Cells[0].Value.ToString());
                controller._criterias.Criterias.RemoveAt(EditingIndex);
                dataGridViewCriteriasData.Rows.Remove(SelectedRow[0]);
                for  (int i = EditingIndex; i < dataGridViewCriteriasData.Rows.Count; ++i)
                {
                    int buf = int.Parse(dataGridViewCriteriasData.Rows[i].Cells[0].Value.ToString());
                    --buf;
                    dataGridViewCriteriasData.Rows[i].Cells[0].Value = buf;
                }
                dataGridViewAlternatives.Columns.RemoveAt(EditingIndex + 1);
            }
        }

        private void dataGridViewAlternatives_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CheckAlterntivesValues()
        {
            controller._alternatives.Alternatives.Clear();
            for (int i = 0; i < dataGridViewAlternatives.Rows.Count -1; ++i)
            {
                AlternativeController alternative = new AlternativeController();
                DataGridViewRow row = dataGridViewAlternatives.Rows[i];
                if (row.Cells[0].Value.ToString() == "")
                {
                    throw new Exception("Title of alterntive #" + (i + 1).ToString() + " is missed");
                }
                alternative.Title = row.Cells[0].Value.ToString();

                for (int j = 0; j < controller._criterias.Criterias.Count; ++j)
                {
                    if (controller._criterias.Criterias[j].CriteriaType == CriteriaTypeEnum.Quantitative)
                    {
                        DataGridViewTextBoxCell cell = (DataGridViewTextBoxCell)row.Cells[j + 1];
                        double value;
                        if (cell.Value.ToString()=="" || double.TryParse(cell.Value.ToString(), out value) == false)
                        {
                            throw new Exception("Value of alternative #" + (i+1).ToString() + " and criteria #" + (j + 1).ToString() + " is invalid.");
                        }
                        if (!(value >= controller._criterias.Criterias[j].MinValue && value <= controller._criterias.Criterias[j].MaxValue))
                        {
                            throw new Exception("Value of alternative #" + (i+1).ToString() + " and criteria #" + (j + 1).ToString() + " doesn't match MinMax interval.");
                        }
                        alternative.Values.Add(value);
                    }
                    else
                    {
                        DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)row.Cells[j + 1];
                        double value = (cell.FormattedValue.ToString() != "" ?
                            QualitativeToDouble[cell.FormattedValue.ToString()] :
                            -1);
                        if (value.ToString() == "-1")
                        {
                            throw new Exception("Value of alternative #" + (i+1).ToString() + " and criteria #" + (j + 1).ToString() + " is invalid.");
                        }
                        if (!(value >= controller._criterias.Criterias[j].MinValue && value <= controller._criterias.Criterias[j].MaxValue))
                        {
                            throw new Exception("Value of alternative #" + (i + 1).ToString() + " and criteria #" + (j + 1).ToString() + " doesn't match MinMax interval.");
                        }
                        alternative.Values.Add(value);
                    }
                }
                controller._alternatives.Alternatives.Add(alternative);
            }
        }

        private void tabControlMain_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (tabControlMain.SelectedIndex == 2)
            {
                try
                {
                    CheckAlterntivesValues();
                    textBoxELECTREBeta.Enabled = true;
                    textBoxELECTREAlpha.Enabled = true;
                    checkBoxPROMETHEE.Enabled = true;
                    foreach (CriteriaController data in controller._criterias.Criterias)
                    {
                        if (data.PROMETHEEConfiguration == SpecialParametersEnum.None)
                        {
//                            checkBoxPROMETHEE.Checked = false;
                            checkBoxPROMETHEE.Enabled = false;
                        }
                        if(data.ELECTREConfiguration == SpecialParametersEnum.None)
                        {
                            textBoxELECTREAlpha.Enabled = false;
                            textBoxELECTREBeta.Enabled = false;
                        }
                    }

                    comboBoxSpecifyWASPAS.SelectedIndex = (int)controller.WASPASConfiguration;
                    if (comboBoxSpecifyWASPAS.SelectedIndex <= 0)
                    {
                        checkBoxWASPAS.Enabled = false;
                        checkBoxWASPAS.Checked = false;                        
                    }
                    if (comboBoxSpecifyWASPAS.SelectedIndex == 1)
                    {
                        textBoxWASPASLambda.Enabled = false;
                        checkBoxWASPAS.Enabled = true;
                    }
                    if (comboBoxSpecifyWASPAS.SelectedIndex == 2)
                    {
                        textBoxWASPASLambda.Enabled = true;
                        textBoxWASPASLambda.Text = controller.WASPASLambda.ToString();
                        checkBoxWASPAS.Enabled = true;
                    }


                }
                catch (Exception E)
                {
                    MessageBox.Show(E.Message.ToString());
                    tabControlMain.SelectedIndex = 1;
                }
            }           
        }

        private void buttonCalc_Click(object sender, EventArgs e)
        {
            try
            {
                CheckWASPASValue();
                CheckELECTREValues();
                dataGridViewRanks.Rows.Clear();
                dataGridViewRanks.Columns.Clear();
                FillColumns();
                foreach (DataGridViewRow row in controller.GetSolutionsAsDataGrid())
                {
                    dataGridViewRanks.Rows.Add(row);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CheckELECTREValues()
        {
            if (textBoxELECTREAlpha.Enabled == false)
            {
                controller.ELECTREAlpha = 0;
                controller.ELECTREBeta = 0;
            }
            else
            {
                double alpha = 0, beta = 0;
                if (double.TryParse(textBoxELECTREAlpha.Text, out alpha) == false)
                {
                    throw new Exception("ELECTRE alpha is invalid");
                }
                if (double.TryParse(textBoxELECTREBeta.Text, out beta) == false)
                {
                    throw new Exception("ELECTRE beta is invalid");
                }
                controller.ELECTREAlpha = alpha;
                controller.ELECTREBeta = beta;
            }
        }
        private void CheckWASPASValue()
        {
            if (comboBoxSpecifyWASPAS.SelectedIndex == 1)
            {
                controller.WASPASLambda = 0.5;
            }
            if (comboBoxSpecifyWASPAS.SelectedIndex == 2)
            {
                double Value;
                if (textBoxWASPASLambda.Text == "" || double.TryParse(textBoxWASPASLambda.Text, out Value) == false)
                {
                    throw new Exception("WASPAS parameter is invalid");
                }
                controller.WASPASLambda = Value;
            }
        }

        private void FillColumns()
        {
            foreach(String columnTitle in controller.GetAlternativesTitles())
            {
                DataGridViewColumn newCol = new DataGridViewColumn();
                newCol.HeaderText = columnTitle;
                dataGridViewRanks.Columns.Add(newCol);
            }
        }

        private void checkBoxChooseAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxChooseAll.Checked == true)
            {
                if (checkBoxPROMETHEE.Enabled == true)
                {
                    checkBoxPROMETHEE.Checked = checkBoxChooseAll.Checked;
                    controller._methods[SolutionController.MethodsEnum.PROMETHEE] = checkBoxChooseAll.Checked;
                }

                if (checkBoxELECTRE.Enabled == true)
                {
                    checkBoxELECTRE.Checked = checkBoxChooseAll.Checked;
                    controller._methods[SolutionController.MethodsEnum.PROMETHEE] = checkBoxChooseAll.Checked;
                }

                checkBoxSMART.Checked = checkBoxChooseAll.Checked;
                controller._methods[SolutionController.MethodsEnum.SMART] = checkBoxChooseAll.Checked;

                checkBoxREGIME.Checked = checkBoxChooseAll.Checked;
                controller._methods[SolutionController.MethodsEnum.REGIME] = checkBoxChooseAll.Checked;

                if (checkBoxWASPAS.Enabled == true)
                {
                    checkBoxWASPAS.Checked = checkBoxChooseAll.Checked;
                    controller._methods[SolutionController.MethodsEnum.WASPAS] = checkBoxChooseAll.Checked;
                }

                checkBoxTAXONOMY.Checked = checkBoxChooseAll.Checked;
                controller._methods[SolutionController.MethodsEnum.TAXONOMY] = checkBoxChooseAll.Checked;
            }
        }

        private void checkBoxSMART_CheckedChanged(object sender, EventArgs e)
        {
            controller._methods[SolutionController.MethodsEnum.SMART] = checkBoxSMART.Checked;
            if (checkBoxSMART.Checked == false)
            {
                checkBoxChooseAll.Checked = false;
            }
        }

        private void checkBoxREGIME_CheckedChanged(object sender, EventArgs e)
        {
            controller._methods[SolutionController.MethodsEnum.REGIME] = checkBoxREGIME.Checked;
            if (checkBoxREGIME.Checked == false)
            {
                checkBoxChooseAll.Checked = false;
            }
        }

        private void checkBoxPROMETHEE_CheckedChanged(object sender, EventArgs e)
        {
            controller._methods[SolutionController.MethodsEnum.PROMETHEE] = checkBoxPROMETHEE.Checked;
            if (checkBoxPROMETHEE.Checked == false)
            {
                checkBoxChooseAll.Checked = false;
            }
        }

        private void checkBoxWASPAS_CheckedChanged(object sender, EventArgs e)
        {
            controller._methods[SolutionController.MethodsEnum.WASPAS] = checkBoxPROMETHEE.Checked;
            if (checkBoxWASPAS.Checked == false)
            {
                checkBoxChooseAll.Checked = false;
            }
        }

        private void checkBoxTAXONOMY_CheckedChanged(object sender, EventArgs e)
        {
            controller._methods[SolutionController.MethodsEnum.TAXONOMY] = checkBoxTAXONOMY.Checked;
            if (checkBoxTAXONOMY.Checked == false)
            {
                checkBoxChooseAll.Checked = false;
            }
        }

        private void checkBoxELECTRE_CheckedChanged(object sender, EventArgs e)
        {
            controller._methods[SolutionController.MethodsEnum.ELECTRE] = checkBoxELECTRE.Checked;
            if (checkBoxELECTRE.Checked == false)
            {
                checkBoxChooseAll.Checked = false;
            }
        }

        private void comboBoxSpecifyWASPAS_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            controller.WASPASConfiguration = (SpecialParametersEnum)comboBoxSpecifyWASPAS.SelectedIndex;
            if (comboBoxSpecifyWASPAS.SelectedIndex <= 0)
            {
                checkBoxWASPAS.Enabled = false;
                checkBoxWASPAS.Checked = false;
            }
            if (comboBoxSpecifyWASPAS.SelectedIndex == 1)
            {
                textBoxWASPASLambda.Enabled = false;
                checkBoxWASPAS.Enabled = true;
            }
            if (comboBoxSpecifyWASPAS.SelectedIndex == 2)
            {
                textBoxWASPASLambda.Enabled = true;
                checkBoxWASPAS.Enabled = true;
            }
        }

        private void comboBoxSpecifyELECTRE_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSpecifyELECTRE.SelectedIndex == 2)
            {
                textBoxElectreP.Enabled = true;
                textBoxElectreP.Text = "";

                textBoxElectreQ.Enabled = true;
                textBoxElectreQ.Text = "";

                textBoxElectreV.Enabled = true;
                textBoxElectreV.Text = "";
            }
            else
            {
                textBoxElectreP.Enabled = true;
                textBoxElectreQ.Enabled = true;
                textBoxElectreV.Enabled = true;
            }
        }

        
    }

}
