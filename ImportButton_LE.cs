using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Futhark {

    public class ImportButton_LE : Button_LE{ 


        public ImportButton_LE(Texture2D texture, int ratio) : base(texture, ratio) {         
            rect =  new Rectangle(12 * ratio, 2 * ratio, 8 * ratio, 4 * ratio);
        }

        public override bool Update(Point mousePos) {
            return base.Update(mousePos);
        }

        public override void Draw (SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
        }

        public override void OnClick() {
            //SaveToBMP
        }

        public string SelectImport() {
            return "";
            // var fileContent = string.Empty;
            // var filePath = string.Empty;

            // using (OpenFileDialog openFileDialog = new OpenFileDialog())
            // {
            //     openFileDialog.InitialDirectory = "c:\\";
            //     openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            //     openFileDialog.FilterIndex = 2;
            //     openFileDialog.RestoreDirectory = true;

            //     if (openFileDialog.ShowDialog() == DialogResult.OK)
            //     {
            //         //Get the path of specified file
            //         filePath = openFileDialog.FileName;

            //         //Read the contents of the file into a stream
            //         var fileStream = openFileDialog.OpenFile();

            //         using (StreamReader reader = new StreamReader(fileStream))
            //         {
            //             fileContent = reader.ReadToEnd();
            //         }
            //     }
            // }

            // MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);
            // return filePath;
        }

        
    }
    
    
}