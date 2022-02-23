using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;

using JPlatform.Client.Library6.interFace;
using JPlatform.Client;
using JPlatform.Client.Controls6;
using JPlatform.Client.JBaseForm6;
using JPlatform.Client.WOORIERPBaseForm6;
using System.Globalization;
using System.IO;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;

namespace JihooSoft.WOORIERP.HRM
{
    
    public partial class HRI11000 : WOORIERPBaseForm
    {
        int max_sub_code = 0;
        private DataNavigator dataNavigator;
        string dept_id = "";
        public HRI11000()
        {
            InitializeComponent();
            pbSetImageResource(btnQ, ImageResourceType.ButtonBackground);
            pbSetImageResource(btnEnroll, ImageResourceType.ButtonBackground);
            pbSetImageResource(btnFileAdd, ImageResourceType.AddButton);
            pbSetImageResource(btnFileDelete, ImageResourceType.DeleteRowButton);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            NewButton = false;
            AddButton = false;
            SaveButton = false;
            DeleteButton = false;
            DeleteRowButton = false;
            PrintButton = false;
            PreviewButton = false;

            string sDate = CurrentDate("");
            ymddate.EditValue = sDate;

            if (SessionInfo.UserCategory == "ADMIN")
            {
                btnEnroll.Enabled = true;
                cboreceiver.Visible = true;
                cbotitle.Visible = true;
            }
            else
            {
                if (SessionInfo.WorkPlaceType.Equals("HQ"))
                {
                    if (FormAccessInfo.form_site_all_yn) //전체 사업장
                    {
                        if(SessionInfo.DutyCode.ToString() == "150")
                        {
                            //본부장 (미결 결재만 가능)
                            gvwFile.Columns["file_category"].OptionsColumn.AllowEdit = false;
                            gvwFile.Columns["docID"].OptionsColumn.AllowEdit = false;
                            gvwFile.Columns["send_date"].OptionsColumn.AllowEdit = false;
                            gvwFile.Columns["receiver_code"].OptionsColumn.AllowEdit = false;
                            gvwFile.Columns["title_code"].OptionsColumn.AllowEdit = false;
                            gvwFile.Columns["sender"].OptionsColumn.AllowEdit = false;
                            gvwFile.Columns["dept_name"].OptionsColumn.AllowEdit = false;
                            gvwFile.Columns["issignok"].OptionsColumn.AllowEdit = true;
                            gvwFile.Columns["title_text"].OptionsColumn.AllowEdit = false;
                        }
                        else if(SessionInfo.DeptId.ToString() == "2017025")
                        {
                            // 총무팀 (입력/수정 가능)
                        }
                        
                    }
                    else if (FormAccessInfo.form_site_group_yn)  //그룹
                    {
                        dept_id = SessionInfo.DeptId.ToString();
                    }
                    else if (FormAccessInfo.form_site_dept_yn)  //조직
                    {
                        dept_id = SessionInfo.DeptId.ToString();  
                    }
                    else
                    {
                        dept_id = SessionInfo.DeptId.ToString();
                    }
                }
                else
                {
                    if (FormAccessInfo.form_site_group_yn)  //그룹 추가
                    {           // 추가
                       
                    }   // 추가
                    else          // 추가
                    {           // 추가
                        
                    }   
                }
                if (!FormAccessInfo.AllowSave)
                {
                    gvwFile.OptionsBehavior.Editable = false;
                }
            }
           

            DateTime today = GetServerDateTime();
            DateTime before6Month = today.AddMonths(-6);
            DateTime after6Month = today.AddMonths(6);

            ymdsend_date_fr.EditValue = before6Month;
            ymdsend_date_to.EditValue = after6Month;
            
            cbofile_category.EditValue = "";
            QueryClick();

            InitLookUp1();
            InitLookUp2();
        }

        #region [Start Button Event Code By UIBuilder]
        public override void QueryClick()
        {

            fnQRY_P_HRI11000_Q("Q");
            SetLookUp(cboreceiver, "", "L_HRI113", "");
            SetLookUp(cbotitle, "", "L_HRI112", "");
            DataTable dt = cboreceiver.Properties.DataSource as DataTable;
            repositoryItemGridLookUpEdit1.DataSource = dt;
            DataTable dt2 = cbotitle.Properties.DataSource as DataTable;
            repositoryItemGridLookUpEdit2.DataSource = dt2;

        }
        public override void SaveClick()
        {
            if (fnSET_P_HRI11000_S("N"))
            {
                SetMessageBox("저장완료하였습니다.");
                QueryClick();
            }
        }

        #endregion [End Button Event]

        #region [Start DB Related Code By UIBuilder]
        //View Code Generated By UIBuilder
        private bool fnQRY_P_HRI11000_Q(string strWorkType)
		{
			if (!ValidateControls(panTop))
				return false;

			try
			{
                // 비즈니스 로직 정보
                P_HRI11000_Q cProc = new P_HRI11000_Q();
				DataTable dtData = null;
                dtData = cProc.SetParamData(dtData,
                                strWorkType,
                                //cbogubun.EditValue.ToString(),
                                ymdsend_date_fr.yyyymmdd,
                                ymdsend_date_to.yyyymmdd,
                                cbofile_category.EditValue.ToString(),
                                dept_id
                                );
				ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo());
                DataTable dt = rs.ResultDataSet.Tables[0];
                DataTable dt2 = rs.ResultDataSet.Tables[1];
                SetData(grdFile, dt);
                max_sub_code = Int32.Parse(dt2.Rows[0][0].ToString());
				cProc = null;
				return true;
			}
			catch (Exception ex)
			{
				SetErrorMessage(ex);
				return false;
			}
		}
        private bool fnSET_P_HRI11000_S(string strWorkType)
        {
            P_HRI11000_S cProc = new P_HRI11000_S();
            DataTable dtSource = BindingData(grdFile, true, false);
            DataTable dtData = null;
            
            if (dtSource.Rows == null)
            {
                return false;
            }
            if (dtSource.Rows.Count == 0)
            {
                return false;
            }
            
            try
            {
                foreach (DataRow dr in dtSource.Rows)
                {
                    dtData = cProc.SetParamData(dtData,
                                     dr["RowStatus"].ToString(),
                                     dr["ID"].ToString() == "" ? 0 : Int64.Parse(dr["ID"].ToString()),
                                     dr["file_category"].ToString(),
                                     dr["file_path"].ToString(),
                                     dr["file_name"].ToString(),
                                     dr["file_type"].ToString(),
                                     dr["send_date"].ToString(),
                                     dr["gubun"].ToString(),
                                     dr["receiver_text"].ToString(),
                                     dr["receiver_code"].ToString() == "" ? 0 : Int32.Parse(dr["receiver_code"].ToString()),
                                     dr["title_text"].ToString(),
                                     dr["title_code"].ToString() == "" ? 0 : Int32.Parse(dr["title_code"].ToString()),
                                     dr["sender_emp_id"].ToString(),
                                     dr["sender"].ToString().Replace("-", ""),
                                     dr["dept_id"].ToString().Replace("-", ""),
                                     dr["dept_name"].ToString(),
                                     dr["issignok"].ToString(),
                                     dr["system_yn"].ToString(),
                                     dr["issignok_altered"].ToString(),

                                     SessionInfo.UserID,
                                     GetClientPCName() + "/" + GetIPAddress()
                                     );
                    //MessageBox.Show(dr["title"].ToString() + " /// " + text);
                }
                
                bool bResult = false;
                ResultSet rs = CommonCallQuery(dtData, cProc.ProcName, cProc.GetParamInfo());

                if (rs != null)
                {
                    bResult = true;
                }
                else
                {
                    bResult = false;
                }

                cProc = null;
                return bResult;
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex);
                return false;
            }
        }
        #endregion [End DB Related Code]
        #region [Procedure Information Class By UIBuilder]
        /// <summary>
        /// Modify Class : Class Name is Procedure Name
        /// </summary>
        public class P_HRI11000_Q : BaseProcClass
		{
			public P_HRI11000_Q()
			{
				// Modify Code : Procedure Name
				_ProcName = "P_HRI11000_Q";
				ParamAdd();
			}

			private void ParamAdd()
			{
				// Modify Code : Procedure Parameter
					_ParamInfo.Add(new ParamInfo("@p_work_type", "Varchar", 10, "Input",typeof(System.String )));
                    _ParamInfo.Add(new ParamInfo("@p_send_date_fr", "Varchar", 8, "Input", typeof(System.String)));
                    _ParamInfo.Add(new ParamInfo("@p_send_date_to", "Varchar", 8, "Input", typeof(System.String)));
                    _ParamInfo.Add(new ParamInfo("@p_file_category", "Varchar", 10, "Input", typeof(System.String)));
                    _ParamInfo.Add(new ParamInfo("@p_dept_id", "Varchar", 20, "Input", typeof(System.String)));

            }

			public DataTable SetParamData(DataTable dataTable,System.String @p_work_type,
										  System.String @p_send_date_fr,
                                          System.String @p_send_date_to,
                                          System.String @p_file_category,
                                          System.String @p_dept_id
                                         )
			{
				if (dataTable == null) 
				{
					dataTable = new DataTable(_ProcName);
					foreach (ParamInfo pi in _ParamInfo)
					{
						dataTable.Columns.Add(pi.ParamName, pi.TypeClass);
					}
				}
				// Modify Code : Procedure Parameter
				object[] objData = new object[] {
					@p_work_type,
                    @p_send_date_fr,
                    @p_send_date_to,
                    @p_file_category,
                    @p_dept_id
                };
				dataTable.Rows.Add(objData);
				return dataTable;
			}
		}

        public class P_HRI11000_S : BaseProcClass
        {
            public P_HRI11000_S()
            {
                // Modify Code : Procedure Name
                _ProcName = "P_HRI11000_S";
                ParamAdd();
            }

            private void ParamAdd()
            {
                // Modify Code : Procedure Parameter
                _ParamInfo.Add(new ParamInfo("@p_work_type", "Varchar", 10, "Input", typeof(System.String)));
                _ParamInfo.Add(new ParamInfo("@p_ID", "Int", 8, "Input", typeof(System.Int64)));
                _ParamInfo.Add(new ParamInfo("@p_file_category", "Varchar", 10, "Input", typeof(System.String)));
                _ParamInfo.Add(new ParamInfo("@p_file_path", "Nvarchar", 100, "Input", typeof(System.String)));
                _ParamInfo.Add(new ParamInfo("@p_file_name", "Nvarchar", 200, "Input", typeof(System.String)));
                _ParamInfo.Add(new ParamInfo("@p_file_type", "Varchar", 10, "Input", typeof(System.String)));
                _ParamInfo.Add(new ParamInfo("@p_send_date", "Varchar", 8, "Input", typeof(System.String)));
                _ParamInfo.Add(new ParamInfo("@p_gubun", "Varchar", 1, "Input", typeof(System.String)));
                _ParamInfo.Add(new ParamInfo("@p_receiver_text", "Nvarchar", 100, "Input", typeof(System.String)));
                _ParamInfo.Add(new ParamInfo("@p_receiver_code", "Int", 4, "Input", typeof(System.Int32)));
                _ParamInfo.Add(new ParamInfo("@p_title_text", "Nvarchar", 100, "Input", typeof(System.String)));
                _ParamInfo.Add(new ParamInfo("@p_title_code", "Int", 4, "Input", typeof(System.Int32)));
                _ParamInfo.Add(new ParamInfo("@p_sender_emp_id", "Varchar", 30, "Input", typeof(System.String)));
                _ParamInfo.Add(new ParamInfo("@p_sender", "Nvarchar", 10, "Input", typeof(System.String)));
                _ParamInfo.Add(new ParamInfo("@p_dept_id", "Varchar", 30, "Input", typeof(System.String)));
                _ParamInfo.Add(new ParamInfo("@p_dept_name", "Nvarchar", 20, "Input", typeof(System.String)));
                _ParamInfo.Add(new ParamInfo("@p_issignok", "Varchar", 1, "Input", typeof(System.String)));
                _ParamInfo.Add(new ParamInfo("@p_system_yn", "Varchar", 1, "Input", typeof(System.String)));
                _ParamInfo.Add(new ParamInfo("@p_issignok_altered", "Varchar", 1, "Input", typeof(System.String)));

                _ParamInfo.Add(new ParamInfo("@p_userid", "Varchar", 30, "Input", typeof(System.String)));
                _ParamInfo.Add(new ParamInfo("@p_pc", "Nvarchar", 200, "Input", typeof(System.String)));
            }

            public DataTable SetParamData(DataTable dataTable, System.String @p_work_type,
                                                               System.Int64  @p_ID,
                                                               System.String @p_file_category,
                                                               System.String @p_file_path,
                                                               System.String @p_file_name,
                                                               System.String @p_file_type,
                                                               System.String @p_send_date,
                                                               System.String @p_gubun,
                                                               System.String @p_receiver_text,
                                                               System.Int32 @p_receiver_code,
                                                               System.String @p_title_text,
                                                               System.Int32 @p_title_code,
                                                               System.String @p_sender_emp_id,
                                                               System.String @p_sender,
                                                               System.String @p_dept_id,
                                                               System.String @p_dept_name,
                                                               System.String @p_issignok,
                                                               System.String @p_system_yn,
                                                               System.String @p_issignok_altered,

                                                               System.String @p_userid,
                                                               System.String @p_pc)

            {
                if (dataTable == null)
                {
                    dataTable = new DataTable(_ProcName);
                    foreach (ParamInfo pi in _ParamInfo)
                    {
                        dataTable.Columns.Add(pi.ParamName, pi.TypeClass);
                    }
                }
                // Modify Code : Procedure Parameter
                object[] objData = new object[] {
                    @p_work_type,
                    @p_ID,
                    @p_file_category,
                    @p_file_path,
                    @p_file_name,
                    @p_file_type,
                    @p_send_date,
                    @p_gubun,
                    @p_receiver_text,
                    @p_receiver_code,
                    @p_title_text,
                    @p_title_code,
                    @p_sender_emp_id,
                    @p_sender,
                    @p_dept_id,
                    @p_dept_name,
                    @p_issignok,
                    @p_system_yn,
                    @p_issignok_altered,
                    @p_userid,
                    @p_pc
                };
                dataTable.Rows.Add(objData);
                return dataTable;
            }
        }

        #endregion [End Procedure Information Class]


        private void InitLookUp1()
        {
            repositoryItemGridLookUpEdit1.ValueMember = "sub_code";
            //repositoryItemGridLookUpEdit1.ValueMember = "code_name";
            repositoryItemGridLookUpEdit1.DisplayMember = "code_name";
            
            repositoryItemGridLookUpEdit1.View.Columns.AddField("sub_code");
            repositoryItemGridLookUpEdit1.View.Columns.AddField("code_name");
            repositoryItemGridLookUpEdit1.View.Columns.AddField("system_yn");
            
            repositoryItemGridLookUpEdit1.View.Columns["code_name"].Visible = true;
            
            repositoryItemGridLookUpEdit1.PopupFilterMode = PopupFilterMode.Contains;
            repositoryItemGridLookUpEdit1.TextEditStyle = TextEditStyles.Standard;
            repositoryItemGridLookUpEdit1.ImmediatePopup = true;
            repositoryItemGridLookUpEdit1.AutoComplete = true;
            DataTable dt = cboreceiver.Properties.DataSource as DataTable;
            repositoryItemGridLookUpEdit1.DataSource = dt;
        }
        private void InitLookUp2()
        {
            repositoryItemGridLookUpEdit2.ValueMember = "sub_code";
            //repositoryItemGridLookUpEdit2.ValueMember = "code_name";
            repositoryItemGridLookUpEdit2.DisplayMember = "code_name";

            repositoryItemGridLookUpEdit2.View.Columns.AddField("sub_code");
            repositoryItemGridLookUpEdit2.View.Columns.AddField("code_name");

            repositoryItemGridLookUpEdit2.View.Columns["code_name"].Visible = true;

            repositoryItemGridLookUpEdit2.PopupFilterMode = PopupFilterMode.Contains;
            repositoryItemGridLookUpEdit2.TextEditStyle = TextEditStyles.Standard;
            
            repositoryItemGridLookUpEdit2.ImmediatePopup = true;
            repositoryItemGridLookUpEdit2.AutoComplete = true;
            DataTable dt = cbotitle.Properties.DataSource as DataTable;
            repositoryItemGridLookUpEdit2.DataSource = dt;
        }

        private void btnQ_MouseHover(object sender, EventArgs e)
        {
            this.btnQ.Appearance.BackColor = Color.Brown;
            this.btnQ.Appearance.ForeColor = Color.White;
        }

        private void btnQ_MouseLeave(object sender, EventArgs e)
        {
            this.btnQ.Appearance.BackColor = Color.Silver;
        }
        private void btnQ_Click(object sender, EventArgs e)
        {
            QueryClick();
        }

        private void btnEnroll_MouseHover(object sender, EventArgs e)
        {
            this.btnEnroll.Appearance.BackColor = Color.Brown;
            this.btnEnroll.Appearance.ForeColor = Color.White;
        }

        private void btnEnroll_MouseLeave(object sender, EventArgs e)
        {
            this.btnEnroll.Appearance.BackColor = Color.Silver;
        }
        private void btnEnroll_Click(object sender, EventArgs e)
        {
            if (fnSET_P_HRI11000_S("N"))
            {
                SetMessageBox("저장완료하였습니다.");
                QueryClick();
            }
        }

        private void btnAutoFiltering_Click(object sender, EventArgs e)
        {
            bool b = gvwFile.OptionsView.ShowAutoFilterRow;

            if (b == true)
            {
                gvwFile.OptionsView.ShowAutoFilterRow = false;
            }
            else
            {
                gvwFile.OptionsView.ShowAutoFilterRow = true;
                gvwFile.Columns["file_name"].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gvwFile.Columns["title_code"].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gvwFile.Columns["receiver_code"].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                
                gvwFile.Columns["insert_time"].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
                gvwFile.Columns["insert_userid"].OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            }
        }

        private void simpleButtonEx1_Click(object sender, EventArgs e)
        {
            //SetGridToExcel("SNC인사관리_전체정보", grdList2, true, ExcelType.XLSX);
            SetGridToExcel("대외공물발송대장_" + ymddate.yyyymmdd, grdFile, true, ExcelType.XLSX);
        }

        private void cbogubun_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            //QueryClick();
        }

        private void repositoryItemButtonEditEx5_Click(object sender, EventArgs e)
        {
            // 파일다운로드
            if (gvwFile.RowCount < 1)
                return;

            string strFilePath = gvwFile.GetValue("file_path").ToString();
            string strFileLocalname = gvwFile.GetValue("file_name").ToString();
            string strfilename = strFilePath;
            int iindex = strFilePath.IndexOf("//");
            strfilename = strfilename.Replace("//", "");

            if (strfilename == "")
                return;

            string FileName = strfilename.Substring(iindex, strfilename.Length - iindex);
            string folderPath = @"C:\대외공문발송대장\";

            DirectoryInfo di = new DirectoryInfo(folderPath);

            if (di.Exists == false)
            {
                di.Create();
            }

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.InitialDirectory = folderPath;
            saveFileDialog1.FileName = FileName;
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.Filter = "All Files|*.*";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                byte[] byteFile = FileDownload(@"HRExternal\\" + FileName);

                if (byteFile == null)
                {
                    SetMessageBox(GetFormMessage("PROJECTBASE_007")); // 해당하는 파일이 서버에 없습니다.
                    return;
                }
                try
                {
                    File.WriteAllBytes(saveFileDialog1.FileName, byteFile);
                    DialogResult dr = SetYesNoMessageBox("다운로드된 파일을 열어보시겠습니까?");

                    if (dr == DialogResult.Yes)
                    {
                        //다운로드 후 파일 실행.
                        System.Diagnostics.Process.Start(saveFileDialog1.FileName);
                    }
                }
                catch (Exception ex)
                {
                    SetErrorMessage(ex);
                }
            }
        }

        private void btnFileAdd_Click(object sender, EventArgs e)
        {
            GridAddNewRow(grdFile);
            gvwFile.SetFocusedRowCellValue("send_date", ymddate.yyyymmdd);
            gvwFile.SetFocusedRowCellValue("file_category", "");
        }

        private void btnFileDelete_Click(object sender, EventArgs e)
        {
            GridDeleteRow(grdFile);
        }

        private void repositoryItemButtonEditEx2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
          
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "All files (*.*)|*.*";
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                try
                {
                    string[] strFile = openFileDialog1.FileNames;
                    string[] strSafeFiles = openFileDialog1.SafeFileNames;

                    for (int i = 0; i < strFile.Length; i++)
                    {
                        if (!pbFileSizeLimitCheck(strFile[i]))
                            return;
                    }

                    for (int j = 0; j < gvwFile.RowCount; j++)
                    {
                        for (int i = 0; i < strSafeFiles.Length; i++)
                        {
                            if (strSafeFiles[i] == gvwFile.GetValue(j, "file_name").ToString())
                            {
                                SetMessageBox(GetFormMessage("PROJECTBASE_006")); // 동일한 파일명이 있습니다. 파일명을 변경하십시오.
                                return;
                            }
                        }
                    }

                    string[] result = new string[2];

                    for (int i = 0; i < strFile.Length; i++)
                    {
                        result = FileUpload(@"HRExternal\\" + ymddate.yyyymmdd + "_" + strSafeFiles[i], File.ReadAllBytes(strFile[i]), false);
                        
                        if (result != null)
                        {
                            if (result[0] == "OK")
                            {
                                gvwFile.SetValue("file_path", result[1].ToString());
                                gvwFile.SetValue("file_name", strSafeFiles[i].ToString());
                                gvwFile.SetValue("file_type", Path.GetExtension(strSafeFiles[i].ToString()));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SetErrorMessage(ex);
                }
            }
        }

        private void repositoryItemGridLookUpEdit1_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {
            if ((string)e.DisplayValue != String.Empty &&  MessageBox.Show(this, "'" + e.DisplayValue.ToString() + "' 를 수신처 리스트에 추가하시겠습니까?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.repositoryItemGridLookUpEdit1.EditValueChanged -= this.repositoryItemGridLookUpEdit1_EditValueChanged;
                GridLookUpEdit editor = sender as GridLookUpEdit;
                DataTable dt = editor.Properties.DataSource as DataTable;
                DataRow dr = dt.NewRow();
                max_sub_code += 1;
                dr[0] = max_sub_code;
                dr[1] = e.DisplayValue.ToString();
                dr[2] = "Y";
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                e.Handled = true;
                gvwFile.SetFocusedRowCellValue("receiver_text", e.DisplayValue.ToString());
                gvwFile.SetFocusedRowCellValue("system_yn", "Y");

                this.repositoryItemGridLookUpEdit1.EditValueChanged += this.repositoryItemGridLookUpEdit1_EditValueChanged;
            }
        }

        private void repositoryItemGridLookUpEdit2_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {


            if((string)e.DisplayValue != String.Empty && MessageBox.Show(this, "'" + e.DisplayValue.ToString() + "' 를 제목 리스트에 추가하시겠습니까?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.repositoryItemGridLookUpEdit2.EditValueChanged -= this.repositoryItemGridLookUpEdit2_EditValueChanged;
                GridLookUpEdit editor = sender as GridLookUpEdit;
                DataTable dt = editor.Properties.DataSource as DataTable;
                int max_idx = 0;

                foreach (DataRow row in dt.Rows) {

                    int rowC = row[0].ToString() == "" ? 0 : Int32.Parse(row[0].ToString());

                    if (max_idx < rowC)
                    {
                        max_idx = rowC;
                    }
                }
                
                DataRow dr = dt.NewRow();
                dr[0] = max_idx + 1;
                dr[1] = e.DisplayValue.ToString();
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                e.Handled = true;
                gvwFile.SetFocusedRowCellValue("title_text", e.DisplayValue.ToString());
                
                this.repositoryItemGridLookUpEdit2.EditValueChanged += this.repositoryItemGridLookUpEdit2_EditValueChanged;
            }
        }
        
      
        private void repositoryItemGridLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            //GridLookUpEdit editor = sender as GridLookUpEdit;
            //gvwFile.SetFocusedRowCellValue("receiver_text", editor.EditValue.ToString());
        }
        private void repositoryItemGridLookUpEdit2_EditValueChanged(object sender, EventArgs e)
        {
            //GridLookUpEdit editor = sender as GridLookUpEdit;
            //gvwFile.SetFocusedRowCellValue("title_text", editor.EditValue.ToString());
        }


        private void repositoryItemLookUpEditEx1_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEditEx editor = sender as LookUpEditEx;
            gvwFile.SetFocusedRowCellValue("receiver_text", editor.Text);
        }

        private void repositoryItemLookUpEditEx2_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEditEx editor = sender as LookUpEditEx;
            gvwFile.SetFocusedRowCellValue("title_text", editor.Text);
        }

        private void repositoryItemGridLookUpEdit1_CloseUp(object sender, CloseUpEventArgs e)
        {
            //var editor = sender as GridLookUpEdit;
            //if(editor != null)
            //{
            //    var enteredLookUpText = editor.Text;
            //    if(e.CloseMode == PopupCloseMode.Immediate)
            //    {
            //        e.Value = enteredLookUpText;
            //        repositoryItemGridLookUpEdit1_ProcessNewValue(sender, new ProcessNewValueEventArgs(enteredLookUpText));
            //    }
            //}
        }

        private void gvwFile_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if(e.Column.FieldName == "issignok")
            {
                if(e.Value.ToString() == "")
                {
                    gvwFile.SetRowCellValue(e.RowHandle, "issignok_altered", "");
                }
                else
                {
                    gvwFile.SetRowCellValue(e.RowHandle, "issignok_altered", e.Value.ToString());
                }
            }
        }

        private void gvwFile_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if(e.FocusedRowHandle < 0)
            {
                return;
            }

            if (SessionInfo.DutyCode.ToString() != "150")
            {
                return;
            }

            string dept_id = gvwFile.GetRowCellValue(e.FocusedRowHandle, "dept_id").ToString();  //GetFocusedRowCellValue("dept_id").ToString();
            
            if (SessionInfo.DeptId.ToString() != dept_id)
            {
                gvwFile.Columns["issignok"].OptionsColumn.AllowEdit = false;
                
            }
            else
            {
                gvwFile.Columns["issignok"].OptionsColumn.AllowEdit = true;
            }
        }
    }
}
