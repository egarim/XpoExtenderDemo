namespace BIT.MetadataBuilder.Module.Controllers
{
    partial class CompiledModelController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.LoadAssembly = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // GetMetadataFromDatabase
            // 
            this.LoadAssembly.Caption = "Load assembly";
            this.LoadAssembly.ConfirmationMessage = null;
            this.LoadAssembly.Id = "b532d984-4031-411c-ba2e-34b4eea500fe";
            this.LoadAssembly.ToolTip = null;
            this.LoadAssembly.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LoadAssembly_Execute);
            // 
            // CompiledModelController
            // 
            this.Actions.Add(this.LoadAssembly);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction LoadAssembly;
    }
}
