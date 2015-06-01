using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        ArrayList lexemes = new ArrayList();
        ArrayList tokens = new ArrayList();
        ArrayList name = new ArrayList();
        ArrayList type = new ArrayList();
        ArrayList value = new ArrayList();

        public Form1()
        {
            InitializeComponent();
            
        }

        public void printing() {
            for (int i = 0; i < lexemes.Count; i++ )
            {
                textBox3.AppendText(lexemes[i]+Environment.NewLine);
                textBox4.AppendText(tokens[i] + Environment.NewLine);
            }

            for (int j = 0; j < name.Count; j++ )
            {
                textBox5.AppendText(name[j] + Environment.NewLine);
            }

            for (int k = 0; k < type.Count; k++)
            {
                textBox6.AppendText(type[k] + Environment.NewLine);
            }

            for (int l = 0; l < value.Count; l++)
            {
                textBox7.AppendText(value[l] + Environment.NewLine);
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            int size = 1;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) {
                string file = openFileDialog1.FileName;
                try
                {
                    string text = File.ReadAllText(file);
                    textBox1.Text = text;
                    size = text.Length;
                }
                catch (Exception) { 
                
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            int size = 1;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                try
                {
                    string text = File.ReadAllText(file);
                    textBox1.Text = text;
                    size = text.Length;
                }
                catch (Exception)
                {

                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            lexemes.Clear();
            tokens.Clear();
            name.Clear();
            type.Clear();
            value.Clear();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.ShowDialog();

            string name = saveFileDialog1.FileName;
            File.WriteAllText(name, textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            lexemes.Clear();
            tokens.Clear();
            name.Clear();
            type.Clear();
            value.Clear();

            Regex r;
            Match m;
            string[] lines;
            
            lines = textBox1.Text.Split('\n');

            if (textBox1.Text.Length > 0)
            {
                foreach (string s in lines) {
                    string line = string.Copy(s);
                    line = line.Trim();

                    Regex noob = new Regex("");
                    Regex yarn = new Regex(@"[A-Za-z][a-zA-Z0-9]+");
                    Regex numbr = new Regex(@"[0-9]+");
                    Regex numbar = new Regex(@"[0-9]+\.[0-9]+");

                    r = new Regex(@"(?<comdec>BTW)\s+(?<val>.*)");
                    m = r.Match(line);
                    if(m.Success){
                        lexemes.Add(m.Groups["comdec"].Value);
                        tokens.Add("Comment Declaration");
                        lexemes.Add(m.Groups["val"].Value);
                        tokens.Add("Comment Value");
                        continue;
                    }

                    r = new Regex(@"(?<comdec>OBTW)\s+(?<val>.*)(?<endcomdec>TLDR)");
                    m = r.Match(line);
                    if (m.Success)
                    {
                        lexemes.Add(m.Groups["comdec"].Value);
                        tokens.Add("Comment Declaration");
                        lexemes.Add(m.Groups["val"].Value);
                        tokens.Add("Comment Value");
                        lexemes.Add(m.Groups["endcomdec"].Value);
                        tokens.Add("End of Comment Declaration");
                        continue;
                    }

                    Regex start = new Regex(@"(?<start>HAI)");
                    Regex end = new Regex(@"(?<end>KTHXBYE)");

                    r = new Regex(@"(?<start>HAI)");
                    m = r.Match(line);
                    if (m.Success)
                    {
                        lexemes.Add(m.Groups["start"].Value);
                        tokens.Add("Code initializer");
                        continue;
                    }

                    r = new Regex(@"(?<end>KTHXBYE)");
                    m = r.Match(line);
                    if (m.Success)
                    {
                        lexemes.Add(m.Groups["end"].Value);
                        tokens.Add("Code terminator");
                        continue;
                    }

                    r = new Regex(@"(?<vardec>I\s+HAS\s+A)\s+(?<ident>[a-zA-Z][a-zA-Z0-9_]*)(\s+(?<determine>ITZ)\s+(?<val>.+))?(\s+(?<comdec>BTW)\s+(?<comval>.*))?");
                    m = r.Match(line);
                    if (m.Success)
                    {
                        lexemes.Add(m.Groups["vardec"].Value);
                        tokens.Add("Variable declaration");
                        lexemes.Add(m.Groups["ident"].Value);
                        tokens.Add("Variable Identifier");

                        if (!m.Groups["determine"].Value.Equals(""))
                        {
                            lexemes.Add(m.Groups["determine"].Value);
                            tokens.Add("Variable Determiner");
                            lexemes.Add(m.Groups["val"].Value);
                            tokens.Add("Value");
                            value.Add(m.Groups["val"].Value);
                            name.Add(m.Groups["ident"].Value);
                                               

                            Match n;
                            r = new Regex(@"[\\""].+[\\""]");
                            n = r.Match(m.Groups["val"].Value);
                            if (n.Success)
                            {
                                type.Add("YARN");
                                continue;
                            }
                          
                            r = new Regex(@"^[-|+]?([0-9]+)$");
                            n = r.Match(m.Groups["val"].Value);
                            if (n.Success)
                             {
                                    type.Add("NUMBR");
                                    continue;
                             }

                            r = new Regex(@"^[-|+]?[0-9]+\.[0-9]+$");
                            n = r.Match(m.Groups["val"].Value);
                            if(n.Success){
                                type.Add("NUMBAR");
                                continue;
                            }

                            r = new Regex(@"[TRUE|FALSE]");
                            n = r.Match(m.Groups["val"].Value);
                            if(n.Success){
                                type.Add("TROOF");
                                continue;
                            }
                        }
                        else {
                            value.Add("NULL");
                            type.Add("NOOB");
                            name.Add(m.Groups["ident"].Value);
                            continue;
                        }
                        if (!m.Groups["comdec"].Value.Equals(""))
                        {
                            lexemes.Add(m.Groups["comdec"].Value);
                            tokens.Add("Comment declaration");
                            lexemes.Add(m.Groups["comval"].Value);
                            tokens.Add("Comment Value");
                        }
                    }

                        r = new Regex(@"(?<ident>[a-zA-Z][a-zA-Z0-9_]*)\s+(?<vardec>R)\s+(?<val>.+)");
                        m = r.Match(line);
                        if(m.Success){
                            lexemes.Add(m.Groups["ident"].Value);
                            tokens.Add("Variable Identifier");
                            lexemes.Add(m.Groups["vardec"].Value);
                            tokens.Add("Variable Declaration");
                            lexemes.Add(m.Groups["val"].Value);
                            tokens.Add("Value");

                            name.Add(m.Groups["ident"].Value);
                            value.Add(m.Groups["val"].Value);

                            Match n;
                            r = new Regex(@"[\\""].+[\\""]");
                            n = r.Match(m.Groups["val"].Value);
                            if (n.Success)
                            {
                                type.Add("YARN");
                                continue;
                            }

                            r = new Regex(@"^[-|+]?([0-9]+)$");
                            n = r.Match(m.Groups["val"].Value);
                            if (n.Success)
                            {
                                type.Add("NUMBR");
                                continue;
                            }

                            r = new Regex(@"^[-|+]?[0-9]+\.[0-9]+$");
                            n = r.Match(m.Groups["val"].Value);
                            if (n.Success)
                            {
                                type.Add("NUMBAR");
                                continue;
                            }

                            r = new Regex(@"[TRUE|FALSE]");
                            n = r.Match(m.Groups["val"].Value);
                            if (n.Success)
                            {
                                type.Add("TROOF");
                                continue;
                            }                            
                        }

                        r = new Regex(@"(?<inputdec>VISIBLE)\s+((?<ident>[a-zA-Z][a-zA-Z0-9_]*))");
                        m = r.Match(line);
                        if(m.Success){
                            lexemes.Add(m.Groups["inputdec"].Value);
                            tokens.Add("Input Declaration");
                            lexemes.Add(m.Groups["ident"].Value);
                            tokens.Add("Variable Identifier");
                            continue;
                        }

                        r = new Regex(@"(?<outputdec>GIMMEH)\s+(?<ident>[a-zA-Z][a-zA-Z0-9_]*)");
                        m = r.Match(line);
                        if(m.Success){
                            lexemes.Add(m.Groups["outputdec"].Value);
                            tokens.Add("Input Declaration");
                            lexemes.Add(m.Groups["ident"].Value);
                            tokens.Add("Variable Identifier");
                            continue;
                        }

                        r = new Regex(@"(?<operator>UPPIN|NERFIN|NOT\s+OF)\s+(?<ident>[a-zA-Z][a-zA-Z0-9_]*)");
                        m = r.Match(line);
                        if(m.Success){
                            lexemes.Add(m.Groups["operator"].Value);
                            tokens.Add("Operator");
                            lexemes.Add(m.Groups["ident"].Value);
                            tokens.Add("Variable Identifier");
                            continue;
                        }

                        r = new Regex(@"(?<operator>SUM|DIFF|PRODUKT|QUOSHUNT|MOD|BIGGR|SMALLR)\s+(?<connector>OF)\s+(?<ident>.[^\s]+)\s+(?<operator_concat>AN)\s+(?<ident2>.[^\s]+)");
                        m = r.Match(line);
                        if(m.Success){
                            lexemes.Add(m.Groups["operator"].Value);
                            tokens.Add("Operator");
                            lexemes.Add(m.Groups["connector"].Value);
                            tokens.Add("Connector");
                            lexemes.Add(m.Groups["ident"].Value);
                            tokens.Add("Variable Identifier");
                            lexemes.Add(m.Groups["operator_concat"].Value);
                            tokens.Add("Operator Concatenator");
                            lexemes.Add(m.Groups["ident2"].Value);
                            tokens.Add("Variable Identifier");

                            name.Add(m.Groups["ident"].Value);
                            name.Add(m.Groups["ident2"].Value);
                            continue;
                        }

                        r = new Regex(@"(?<operator>BOTH\s+OF|EITHER\s+OF|WON\s+OF)\s+(?<ident>.[^\s]+)\s+(?<operator_concat>AN)\s+(?<ident2>.[^\s]+)");
                        m = r.Match(line);
                        if(m.Success){
                            lexemes.Add(m.Groups["operator"].Value);
                            tokens.Add("Operator");
                            lexemes.Add(m.Groups["ident"].Value);
                            tokens.Add("Variable Identifier");
                            lexemes.Add(m.Groups["operator_concat"].Value);
                            tokens.Add("Operator Concatenator");
                            lexemes.Add(m.Groups["ident2"].Value);
                            tokens.Add("Variable Identifier");
                            continue;
                        }

                        r = new Regex(@"(?<operator>ANY\s+OF|ALL\s+OF)\s+(?<ident>.[^\s]+)\s+(?<operator_concat>AN)\s+(?<ident2>.[^\s]+)?(\s+(?<operator_concat>AN\s+?<ident3>.+)*?\s+(?<operator2>MKAY))*");
                        m = r.Match(line);
                        if(m.Success){
                            lexemes.Add(m.Groups["operator"].Value);
                            tokens.Add("Operator");
                            lexemes.Add(m.Groups["ident"].Value);
                            tokens.Add("Variable Identifier");
                            lexemes.Add(m.Groups["operator_concat"].Value);
                            tokens.Add("Operator Concatenator");
                            lexemes.Add(m.Groups["ident2"].Value);
                            tokens.Add("Variable Identifier");
                            continue;
                        }

                        r = new Regex(@"(?<operator>BOTH\s+SAEM|DIFFRINT)\s(?<ident>.+)\s+(?<operator_concat>AN)\s+(?<ident2>.[^\s]+)");
                        m = r.Match(line);
                        if(m.Success){
                            lexemes.Add(m.Groups["operator"].Value);
                            tokens.Add("Operator");
                            lexemes.Add(m.Groups["ident"].Value);
                            tokens.Add("Variable Identifier");
                            lexemes.Add(m.Groups["operator_concat"].Value);
                            tokens.Add("Operator Concatenator");
                            lexemes.Add(m.Groups["ident2"].Value);
                            tokens.Add("Variable Identifier");
                            continue;
                        }

                        r = new Regex(@"(?<operator>BOTH\s+SAEM|DIFFRINT)\s(?<ident>.[^\s]+)\s+(?<operator_concat>AND)\s+(?<operator2>BIGGR\s+OF|SMALLR\s+OF)\s+(?<ident2>.[^\s]+)\s+(?<operator_concat>AN)\s+(?<ident3>.[^\s]+)");
                        m = r.Match(line);
                        if(m.Success){
                            lexemes.Add(m.Groups["operator"].Value);
                            tokens.Add("Operator");
                            lexemes.Add(m.Groups["ident"].Value);
                            tokens.Add("Variable Identifier");
                            lexemes.Add(m.Groups["operator_concat"].Value);
                            tokens.Add("Operator Concatenator");
                            lexemes.Add(m.Groups["operator2"].Value);
                            tokens.Add("Operator");
                            lexemes.Add(m.Groups["ident2"].Value);
                            tokens.Add("Variable Identifier");
                        }

                        r = new Regex(@"(?<operator>MAEK)\s+(?<ident>.+)\s+(?<type>TROOF|YARN|NUMBR|NUMBAR|NOOB)");
                        m = r.Match(line);
                        if (m.Success)
                        {
                            lexemes.Add(m.Groups["operator"].Value);
                            tokens.Add("Operator");
                            lexemes.Add(m.Groups["ident"].Value);
                            tokens.Add("Identifier");
                            lexemes.Add(m.Groups["type"].Value);
                            tokens.Add("Type");
                            continue;
                        }

                        r = new Regex(@"(?<ident>[a-zA-Z][a-zA-Z0-9]*)\s+(?<determine>IS\s+NOW\s+A)\s+(?<type>TROOF|YARN|NUMBR|NUMBAR|NOOB)");
                        m = r.Match(line);
                        if (m.Success)
                        {
                            lexemes.Add(m.Groups["ident"].Value);
                            tokens.Add("Identifier");
                            lexemes.Add(m.Groups["determine"].Value);
                            tokens.Add("Determiner");
                            lexemes.Add(m.Groups["type"].Value);
                            tokens.Add("Type");
                            continue;
                        }

                        r = new Regex(@"(?<ident>[a-zA-Z][a-zA-Z0-9]*)\s+(?<typecast>R\s+MAEK)\s+(?<ident2>[a-zA-Z][a-zA-Z0-9]*)\s+(?<determine>A)\s+(?<type>TROOF|YARN|NUMBR|NUMBAR|NOOB)");
                        m = r.Match(line);
                        if (m.Success)
                        {
                            lexemes.Add(m.Groups["ident"].Value);
                            tokens.Add("Identifier");
                            lexemes.Add(m.Groups["typecast"].Value);
                            tokens.Add("Typecast_Declaration");
                            lexemes.Add(m.Groups["ident2"].Value);
                            tokens.Add("Identifier");
                            lexemes.Add(m.Groups["determine"].Value);
                            tokens.Add("Type");
                            continue;
                        }

                        r = new Regex(@"(?<break>GTFO)");
                        m = r.Match(line);
                        if (m.Success)
                        {
                            lexemes.Add(m.Groups["break"].Value);
                            tokens.Add("Break Statement");
                            continue;
                        }

                        r = new Regex(@"(?<funcdec>HOW DUZ I)\s+(?<label>.[^\s]+)");
                        m = r.Match(line);
                        if (m.Success)
                        {
                            lexemes.Add(m.Groups["funcdec"].Value);
                            tokens.Add("Function Declaration");
                            lexemes.Add(m.Groups["label"].Value);
                            tokens.Add("Function Label");

                            Match n;
                            r = new Regex(@"(?<inputdec>VISIBLE)\s+((?<ident>([a-zA-Z][a-zA-Z0-9_]*)|([\\""].+[\\""])))");
                            n = r.Match(line);
                            if (n.Success)
                            {
                                lexemes.Add(m.Groups["inputdec"].Value);
                                tokens.Add("Input Declaration");
                                lexemes.Add(m.Groups["ident"].Value);
                                tokens.Add("Variable Identifier");
                                continue;
                            }

                            r = new Regex(@"(?<funcend>IF U SAY SO)");
                            n = r.Match(line);
                            if (n.Success)
                            {
                                lexemes.Add(m.Groups["funcend"].Value);
                                tokens.Add("Function End Statement");
                                continue;
                            }
                        }

                    }

              
                printing();
            }
            else {
                MessageBox.Show(
                    "Invalid input! Please enter a code.",
                    "Alert",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1
                    );
            }
        }

        private void aBOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Project Name: LOLCODE Interpreter\n\nCreators:\n\tNathalie D. Fule\n\tChristine Faith C. Gabuya\n\tMichael C. Naynes\n\n(C) Copyright 2014 All Rights reserved.", 
                "About",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1
               );
        }
    }
}
