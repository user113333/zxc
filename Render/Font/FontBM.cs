using System;
using System.Xml;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace zxc {
    public class FontBM : Font {
        public float CharacterHeight = 0;


        // TODO: Update - this is Weak
        // TODO: TTF probably works but not sure for this
        // 1. check document elements 0 & 1 values (info & common)
        public FontBM(string path) {
            XmlDocument document = new XmlDocument();

            string fullPath = AppDomain.CurrentDomain.BaseDirectory + "Content/" + path;
            document.Load(fullPath);

            foreach(XmlNode node in document.DocumentElement.ChildNodes[3].ChildNodes){
                Character character = new Character() {
                    Page = Textures.Count + GetInt("page"),
                    Id = GetInt("id"),
                    X = GetInt("x"),
                    Y = GetInt("y"),
                    Width = GetInt("width"),
                    Height = GetInt("height"),
                    XOffset = GetFloat("xoffset"),
                    YOffset = GetFloat("yoffset"),
                    XAdvance = GetFloat("xadvance"),
                };

                Characters.Add((char)character.Id, character);

                // To get the heighest character
                if (character.Height > CharacterHeight) {
                    CharacterHeight = character.Height;
                }
                
                float GetFloat(string attrib) {
                    return float.Parse(node.Attributes[attrib].Value);
                }

                int GetInt(string attrib) {
                    return int.Parse(node.Attributes[attrib].Value);
                }
            }

            foreach(XmlNode page in document.DocumentElement.ChildNodes[2].ChildNodes) {
                string texName = page.Attributes["file"].Value.Split('.')[0];
                var tex = Core.GlobalContent.Load<Texture2D>(fullPath.Substring(0, fullPath.LastIndexOf('/')) + "/" + texName);
                tex.ReplaceColor(Color.Black, Color.Transparent);
                Textures.Add(tex);
            }
        }
    }
}
