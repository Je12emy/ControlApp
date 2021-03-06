﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ControlApp.Entities.Objects;
using ControlApp.ApiCore.Management;
using MetroFramework;
using static ControlApp.OnPremises.Program;

namespace ControlApp.OnPremises.Panels.Admin
{
    public partial class pnlAdminArea : pnlSlider
    {
        AreaManagement ApiAccess = new AreaManagement();
        DepartamentManagement ApiAccessDpt = new DepartamentManagement();
        Area ObjArea = new Area();
        string pIdSession = MystaticValues.IdSession;
        public pnlAdminArea(Form owner) : base(owner)
        {
            InitializeComponent();
            this.StyleManager.Update();
        }
        private void pnlAdminArea_Load(object sender, EventArgs e)
        {
            LoadDataGrid();
            LoadCbDpt(cbDpt_Id);
            btnUpdate.Enabled = false;
            btnActivate.Enabled = false;
            btnDelete.Enabled = false;
        }
        public void CleanFields()
        {
            txtAreaname.Text = "";
            btnUpdate.Enabled = false;
            btnActivate.Enabled = false;
            btnDelete.Enabled = false;
            cbDpt_Id.SelectedIndex = -1;
        }
        private void LoadDataGrid()
        {
            try
            {
                dgvArea.Rows.Clear();
                var ListArea = ApiAccess.SuperRetrieveArea<Area>();
                foreach (Area element in ListArea)
                {
                    string[] RowArea;
                    RowArea = new string[] { element.ID_Area.ToString(),element.Name_Dpt, element.Area_name, element.State.ToString(),
                    element.CreateBy.ToString(), element.UpdateBy.ToString(), element.CreateDate.ToString(),element.UpdateDate.ToString()};
                    dgvArea.Rows.Add(RowArea);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private bool Checkname(string pAreaName)
        {
            try
            {
                bool finded = false;
                ObjArea.Area_name = pAreaName;
                var ListArea = ApiAccess.RetrieveAllByNameArea<Area>(ObjArea);
                foreach (Area element in ListArea)
                {
                    if (element.Area_name == pAreaName)
                    {
                        finded = true;
                        if (finded == true)
                        {
                            break;
                        }
                    }
                }
                return finded;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int Row = dgvArea.CurrentRow.Index;
            string AreaName = dgvArea[1, Row].Value.ToString();
            if (dgvArea[1, Row].Value == null)
            {
                MetroMessageBox.Show(this, "Debe Seleccionar Al menos Algún Valor para Inactivar. \n Favor Intentelo Nuevamente", "Error en Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvArea.Focus();
                return;
            }
                else
                {
                    if (MetroFramework.MetroMessageBox.Show(this, "¿Desea Eliminar el Area de: " + AreaName + "?", "Confirmación de Acción", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        try
                        {
                            ObjArea.ID_Area = Convert.ToInt32(dgvArea[0, Row].Value);
                            ObjArea.UpdateBy = pIdSession;
                            ApiAccess.DeleteArea(ObjArea);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        CleanFields();
                        LoadDataGrid();
                    }
            }
            
           
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string AreaName = txtAreaname.Text;
            if (AreaName.Trim() == string.Empty)
            {
                MetroMessageBox.Show(this, "El Nombre -" + AreaName + "- no es Valido. \n Favor Digite un Nombre Valido", "Error en Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAreaname.Focus();
                return;
            }
                else
                {
                    try
                    {
                    int Row = dgvArea.CurrentRow.Index;
                    ObjArea.ID_Area = Convert.ToInt32(dgvArea[0, Row].Value);
                    ObjArea.ID_Dpt = GetIDDpt();
                    ObjArea.UpdateBy = pIdSession;
                    ObjArea.Area_name = txtAreaname.Text;
                    ApiAccess.UpdateArea(ObjArea);
                }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            CleanFields();
            LoadDataGrid();
        }
        private void btnActivate_Click(object sender, EventArgs e)
        {
            int Row = dgvArea.CurrentRow.Index;
            string AreaName = dgvArea[1, Row].Value.ToString();
            if (MetroFramework.MetroMessageBox.Show(this, "¿Desea Activar el Area de: " + AreaName + "?", "Confirmación de Acción", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    ObjArea.ID_Area = Convert.ToInt32(dgvArea[0, Row].Value);
                    ObjArea.IdSession = pIdSession;
                    ApiAccess.ActivateArea(ObjArea);
                }
                catch (Exception)
                {
                    throw;
                }
                CleanFields();
                LoadDataGrid();
            }
        }
        private void btnCreate_Click(object sender, EventArgs e)
        {
            string AreaName = txtAreaname.Text;
            if (Checkname(AreaName) == true || AreaName.Trim() == string.Empty)
            {
                MetroMessageBox.Show(this, "El Nombre -" + AreaName + "- no es Valido. \n Favor Digite un Nombre Valido", "Error en Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAreaname.Focus();
            }
            else
            {
                try
                {
                    ObjArea.Area_name = AreaName;
                    ObjArea.CreateBy = pIdSession;
                    ApiAccess.CreateArea(ObjArea);
                }
                catch (Exception)
                {
                    throw;
                }
                CleanFields();
                LoadDataGrid();
            }
        }
        private void txtRetrieveByName_TextChanged(object sender, EventArgs e)
        {
            if (txtRetrieveByName.Text == "")
            {
                LoadDataGrid();
                CleanFields();
            }
            else
            {
                try
                {
                    dgvArea.Rows.Clear();
                    ObjArea.Area_name = txtRetrieveByName.Text;
                    var ListArea = ApiAccess.SuperRetrieveAllByNameArea<Area>(ObjArea);
                    foreach (Area element in ListArea)
                    {
                        string[] RowArea;
                        RowArea = new string[] { element.ID_Area.ToString(),element.Name_Dpt, element.Area_name, element.State.ToString(),
                    element.CreateBy.ToString(), element.UpdateBy.ToString(), element.CreateDate.ToString(),element.UpdateDate.ToString()};
                        dgvArea.Rows.Add(RowArea);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        private void dgvArea_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int Row = dgvArea.CurrentRow.Index;
                cbDpt_Id.Text = dgvArea[1, Row].Value.ToString();
                txtAreaname.Text = dgvArea[2, Row].Value.ToString();
                btnUpdate.Enabled = true;
                btnActivate.Enabled = true;
                btnDelete.Enabled = true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            CleanFields();
        }
        private int GetIDDpt()
        {
            int IdDpt = 0;
            try
            {
                var NameDpt = cbDpt_Id.Text;
                var ListDpt = ApiAccessDpt.RetrieveAllDepartament<Departament>();
                foreach (Departament element in ListDpt)
                {
                    if (NameDpt == element.Name_Dpt)
                    {
                        IdDpt = element.ID_Dpt;
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, "Ha ocurrido un error:" + ex + "Favor Comunicarse con el equipo de Sistemas",
                    "Error en Acción", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return IdDpt;
        }
        private void LoadCbDpt(ComboBox cb)
        {
            try
            {
                var ListDpt = ApiAccessDpt.RetrieveAllDepartament<Departament>();
                foreach (Departament element in ListDpt)
                {
                    cb.Items.Add(element.Name_Dpt);
                }
            }
            catch (Exception ex)
            {
                MetroMessageBox.Show(this, "Ha ocurrido un error:" + ex + "Favor Comunicarse con el equipo de Sistemas",
                    "Error en Acción", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
