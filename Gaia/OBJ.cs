// partial info get from Stefan Gordon:
// https://github.com/stefangordon/ObjParser

using System;
using System.Text;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static Gaia.Globals;


namespace Gaia
{

    // ///////////////////////////////////////////////////////////
    // OBJ 3D TYPES
    // ///////////////////////////////////////////////////////////
    interface IType
    {
        void LoadFromStringArray(string[] data);
    }


    public class Group : IType
    {
        public const int MinimumDataLength = 1;
        public const string Prefix = "g";

        public string objName { get; set; }

        public int i_objBlock, i_objMesh;

        public void LoadFromStringArray(string[] data)
        {

            if (data.Length < MinimumDataLength)
                throw new ArgumentException("Input array must be of minimum length " + MinimumDataLength, "data.");

            if (!data[0].ToLower().Equals(Prefix))
                throw new ArgumentException("Data prefix must be '" + Prefix + "'", "data");

            Regex regex = new Regex(@"Block\d\d_Mesh\d\d");
            if (!regex.Match(data[1]).Success)
                throw new ArgumentException("Group must have a format like 'BlockXX_MeshYY' where XX/YY are numbers.");

            objName = data[1];

            i_objBlock = Int32.Parse(objName.Split('_')[0].Replace("Block", ""));
            i_objMesh = Int32.Parse(objName.Split('_')[1].Replace("Mesh", ""));

        }

        public override string ToString()
        {
            return string.Format("o {0}", objName);
        }
    }

    public class Vertex : IType
    {
        public const int MinimumDataLength = 4;
        public const string Prefix = "v";

        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public int Index { get; set; }

        public void LoadFromStringArray(string[] data)
        {
            if (data.Length < MinimumDataLength)
                throw new ArgumentException("Input array must be of minimum length " + MinimumDataLength, "data");

            if (!data[0].ToLower().Equals(Prefix))
                throw new ArgumentException("Data prefix must be '" + Prefix + "'", "data");

            bool success;

            double x, y, z;

            success = double.TryParse(data[1], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
            if (!success) throw new ArgumentException("Could not parse X parameter as double");

            success = double.TryParse(data[2], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
            if (!success) throw new ArgumentException("Could not parse Y parameter as double");

            success = double.TryParse(data[3], NumberStyles.Any, CultureInfo.InvariantCulture, out z);
            if (!success) throw new ArgumentException("Could not parse Z parameter as double");

            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.GetCultureInfo("en-US"), "v {0:F6} {1:F6} {2:F6}", X, Y, Z);
        }
    }


    public class VertexNormal : IType
    {
        public const int MinimumDataLength = 4;
        public const string Prefix = "vn";

        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public int Index { get; set; }

        public void LoadFromStringArray(string[] data)
        {
            if (data.Length < MinimumDataLength)
                throw new ArgumentException("Input array must be of minimum length " + MinimumDataLength, "data");

            if (!data[0].ToLower().Equals(Prefix))
                throw new ArgumentException("Data prefix must be '" + Prefix + "'", "data");

            bool success;

            double x, y, z;

            success = double.TryParse(data[1], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
            if (!success) throw new ArgumentException("Could not parse X parameter as double");

            success = double.TryParse(data[2], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
            if (!success) throw new ArgumentException("Could not parse Y parameter as double");

            success = double.TryParse(data[3], NumberStyles.Any, CultureInfo.InvariantCulture, out z);
            if (!success) throw new ArgumentException("Could not parse Z parameter as double");

            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.GetCultureInfo("en-US"), "vn {0:F6} {1:F6} {2:F6}", X, Y, Z);
        }
    }


    public class Material : IType
    {
        public string Name { get; set; }
        public Color AmbientReflectivity { get; set; }
        public Color DiffuseReflectivity { get; set; }
        public Color SpecularReflectivity { get; set; }
        public Color TransmissionFilter { get; set; }
        public Color EmissiveCoefficient { get; set; }
        public float SpecularExponent { get; set; }
        public float OpticalDensity { get; set; }
        public float Dissolve { get; set; }
        public float IlluminationModel { get; set; }

        public Material()
        {
            this.Name = "DefaultMaterial";
            this.AmbientReflectivity = new Color();
            this.DiffuseReflectivity = new Color();
            this.SpecularReflectivity = new Color();
            this.TransmissionFilter = new Color();
            this.EmissiveCoefficient = new Color();
            this.SpecularExponent = 0;
            this.OpticalDensity = 1.0f;
            this.Dissolve = 1.0f;
            this.IlluminationModel = 0;
        }

        public void LoadFromStringArray(string[] data)
        {
        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            b.AppendLine("newmtl " + Name);

            b.AppendLine(string.Format("Ka {0}", AmbientReflectivity));
            b.AppendLine(string.Format("Kd {0}", DiffuseReflectivity));
            b.AppendLine(string.Format("Ks {0}", SpecularReflectivity));
            b.AppendLine(string.Format("Tf {0}", TransmissionFilter));
            b.AppendLine(string.Format("Ke {0}", EmissiveCoefficient));
            b.AppendLine(string.Format("Ns {0}", SpecularExponent));
            b.AppendLine(string.Format("Ni {0}", OpticalDensity));
            b.AppendLine(string.Format("d {0}", Dissolve));
            b.AppendLine(string.Format("illum {0}", IlluminationModel));

            return b.ToString();
        }
    }


    public class VertexTexture : IType
    {
        public const int MinimumDataLength = 3;
        public const string Prefix = "vt";

        public double X { get; set; }

        public double Y { get; set; }

        public int Index { get; set; }

        public void LoadFromStringArray(string[] data)
        {
            if (data.Length < MinimumDataLength)
                throw new ArgumentException("Input array must be of minimum length " + MinimumDataLength, "data");

            if (!data[0].ToLower().Equals(Prefix))
                throw new ArgumentException("Data prefix must be '" + Prefix + "'", "data");

            bool success;

            double x, y;

            success = double.TryParse(data[1], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
            if (!success) throw new ArgumentException("Could not parse X parameter as double");

            success = double.TryParse(data[2], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
            if (!success) throw new ArgumentException("Could not parse Y parameter as double");
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.GetCultureInfo("en-US"), "vt {0:F6} {1:F6}", X, Y);
        }
    }


    public class Color : IType
    {
        public float r { get; set; }
        public float g { get; set; }
        public float b { get; set; }

        public Color()
        {
            this.r = 1f;
            this.g = 1f;
            this.b = 1f;
        }

        public void LoadFromStringArray(string[] data)
        {
            if (data.Length != 4) return;
            r = float.Parse(data[1]);
            g = float.Parse(data[2]);
            b = float.Parse(data[3]);
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", r, g, b);
        }
    }

    public class Face : IType
    {
        public const int MinimumDataLength = 4;
        public const string Prefix = "f";

        public string UseMtl { get; set; }
        public int[] VertexIndexList;
        public int[] VertexTextureIndexList;
        public int[] VertexNormalIndexList;

        public int i_block;
        public int i_mesh;

        public void LoadFromStringArray(string[] data)
        {
            if (data.Length < MinimumDataLength)
                throw new ArgumentException("Input array must be of minimum length " + MinimumDataLength, "data");

            if (!data[0].ToLower().Equals(Prefix))
                throw new ArgumentException("Data prefix must be '" + Prefix + "'", "data");

            int vcount = data.Length - 1;
            VertexIndexList = new int[vcount];
            VertexTextureIndexList = new int[vcount];
            VertexNormalIndexList = new int[vcount];

            bool success;

            for (int i = 0; i < vcount; i++)
            {
                string[] parts = data[i + 1].Split('/');

                int vindex;
                success = int.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out vindex);
                if (!success) throw new ArgumentException("Could not parse parameter as int");
                VertexIndexList[i] = vindex;

                if (parts.Length > 1)
                {
                    success = int.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out vindex);
                    if (success)
                    {
                        VertexTextureIndexList[i] = vindex;
                    }
                }

                if (parts.Length > 2)
                {
                    success = int.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out vindex);
                    if (success)
                    {
                        VertexNormalIndexList[i] = vindex;
                    }
                }
            }
        }


        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            b.Append("f");

            for (int i = 0; i < VertexIndexList.Length; i++)
            {
                b.AppendFormat(" {0}/{1}/{2}", VertexIndexList[i], VertexTextureIndexList[i], VertexNormalIndexList[i]);
            }

            return b.ToString();
        }
    }


    // ///////////////////////////////////////////////////////////
    // OBJ MAIN CLASS
    // ///////////////////////////////////////////////////////////
    public class OBJ 
    {
        // Define OBJ structure for work with MAP files
        // Normally we would have a list of vertex/normal vertex/texture vertex/faces,
        // but we will create a structure based on groups to have it organized
        // Each group will be composed of Block_XX_Mesh_YY of the MAP
      
        public struct ST_MESH
        {
            public string str_groupName { get; set; }
            public List<Vertex> VList { get; set; }
            public List<VertexNormal> VNList { get; set; }
            public List<Face> FList { get; set; }
            public List<VertexTexture> VTList { get; set; }       
        }


        // ///////////////////////////////////////////////////////////
        // OBJ GLOBAL VARS
        // ///////////////////////////////////////////////////////////
        public struct ST_BLOCK
        {
            public List<ST_MESH> stMesh { get; set; }
        }

        public string str_objmtlfile { get; set; }

        List<Vertex> VAllList;
        List<VertexNormal> VNAllList;
        List<VertexTexture> VTAllList;
        List<Face> FAllList;
        public List<ST_BLOCK> stOBJ { get; set; }


        // ///////////////////////////////////////////////////////////
        // OBJ WRITING
        // ///////////////////////////////////////////////////////////

        public int WriteFullObjFile()
        {
            int i_result;
            int i_block, i_mesh;

            i_result = 0;

            Console.WriteLine("Writing FULL .OBJ worldmap file...");

            try
            {
                using (StreamWriter writerObj = new StreamWriter(str_filename + ".obj"))
                {
                    // write some header data
                    writerObj.WriteLine("# Gaia OBJ file exported from: " + str_filename.ToUpper() + ".MAP");
                    writerObj.WriteLine("");

                    // write .mtl file in mtllib element
                    writerObj.WriteLine("mtllib " + str_objmtlfile);
                    writerObj.WriteLine("");

                    for (i_block = 0; i_block < stOBJ.Count; i_block++)
                    {
                        for (i_mesh = 0; i_mesh < stOBJ[i_block].stMesh.Count; i_mesh++)
                        {
                            // write comment of Block and Mesh
                            writerObj.WriteLine("#");
                            writerObj.WriteLine("# Block" + i_block.ToString("00") + "_Mesh" + i_mesh.ToString("00"));
                            writerObj.WriteLine("#");

                            // write vertices
                            stOBJ[i_block].stMesh[i_mesh].VList.ForEach(v => writerObj.WriteLine(v));
                            // comment with the number of vertices
                            writerObj.WriteLine("# Vertices: " + stOBJ[i_block].stMesh[i_mesh].VList.Count);
                            writerObj.WriteLine("");

                            // write vertex normals
                            stOBJ[i_block].stMesh[i_mesh].VNList.ForEach(vn => writerObj.WriteLine(vn));
                            // comment with the number of vertices
                            writerObj.WriteLine("# Vertex Normals: " + stOBJ[i_block].stMesh[i_mesh].VNList.Count);
                            writerObj.WriteLine("");

                            // write vertex textures
                            stOBJ[i_block].stMesh[i_mesh].VTList.ForEach(vt => writerObj.WriteLine(vt));
                            // comment with the number of vertices
                            writerObj.WriteLine("# Vertex Textures: " + stOBJ[i_block].stMesh[i_mesh].VTList.Count);
                            writerObj.WriteLine("");


                            // write group
                            writerObj.WriteLine("g " + stOBJ[i_block].stMesh[i_mesh].str_groupName);
                            // write faces
                            string lastUseMtl = "";
                            foreach (Face f in stOBJ[i_block].stMesh[i_mesh].FList)
                            {
                                if (f.UseMtl != null && !f.UseMtl.Equals(lastUseMtl))
                                {
                                    writerObj.WriteLine("usemtl " + f.UseMtl);
                                    lastUseMtl = f.UseMtl;
                                }

                                writerObj.WriteLine(f);
                            }
                            // comment with the number of vertices
                            writerObj.WriteLine("# Faces: " + stOBJ[i_block].stMesh[i_mesh].FList.Count);
                            writerObj.WriteLine("");

                        }
                    }
                }
            }
            catch
            {
                i_result = -1;
            }

            return i_result;
        }

        public int WriteByBlocksObjFile()
        {
            int i_result;
            int i_block, i_mesh;

            i_result = 0;

            Console.WriteLine("Writing BY BLOCKS .OBJ worldmap file...");

            try
            {
                for (i_block = 0; i_block < stOBJ.Count; i_block++)
                {
                    using (StreamWriter writerObj = new StreamWriter(str_blockspath + "\\" + str_filename +
                                                                                       "_" + i_block.ToString("00") +
                                                                                       ".obj"))
                    {
                        // write some header data
                        writerObj.WriteLine("# Gaia OBJ file exported from: " + str_filename.ToUpper() + ".MAP (By Blocks)");
                        writerObj.WriteLine("");

                        // write .mtl file in mtllib element
                        writerObj.WriteLine("mtllib " + str_filename + "_" + i_block.ToString("00") + ".mtl");
                        writerObj.WriteLine("");

                        for (i_mesh = 0; i_mesh < stOBJ[i_block].stMesh.Count; i_mesh++)
                        {
                            // some console text
                            Console.Write("\rWriting Block: " + (i_block + 1).ToString("00") + "/" + i_nblocks.ToString("00") +
                                                    ", Mesh: " + (i_mesh + 1).ToString("00") + "/" + N_MESHES.ToString("00") + "...");

                            // write vertices
                            stOBJ[i_block].stMesh[i_mesh].VList.ForEach(v => writerObj.WriteLine(v));
                            // comment with the number of vertices
                            writerObj.WriteLine("# Vertices: " + stOBJ[i_block].stMesh[i_mesh].VList.Count);
                            writerObj.WriteLine("");

                            // write vertex normals
                            stOBJ[i_block].stMesh[i_mesh].VNList.ForEach(vn => writerObj.WriteLine(vn));
                            // comment with the number of vertices
                            writerObj.WriteLine("# Vertex Normals: " + stOBJ[i_block].stMesh[i_mesh].VNList.Count);
                            writerObj.WriteLine("");

                            // write vertex textures
                            stOBJ[i_block].stMesh[i_mesh].VTList.ForEach(vt => writerObj.WriteLine(vt));
                            // comment with the number of vertices
                            writerObj.WriteLine("# Vertex Textures: " + stOBJ[i_block].stMesh[i_mesh].VTList.Count);
                            writerObj.WriteLine("");


                            // write group
                            writerObj.WriteLine("g " + stOBJ[i_block].stMesh[i_mesh].str_groupName);
                            // write faces
                            string lastUseMtl = "";
                            foreach (Face f in stOBJ[i_block].stMesh[i_mesh].FList)
                            {
                                if (f.UseMtl != null && !f.UseMtl.Equals(lastUseMtl))
                                {
                                    writerObj.WriteLine("usemtl " + f.UseMtl);
                                    lastUseMtl = f.UseMtl;
                                }

                                writerObj.WriteLine(f);
                            }
                            // comment with the number of vertices
                            writerObj.WriteLine("# Faces: " + stOBJ[i_block].stMesh[i_mesh].FList.Count);
                            writerObj.WriteLine("");

                        }
                    }
                }

                Console.WriteLine("\r");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("The directory for output the .obj files doesn't exists.");
                i_result = -2;
            }
            catch
            {
                i_result = -1;
            }

            return i_result;
        }


        // ///////////////////////////////////////////////////////////
        // OBJ LOADING & PARSING
        // ///////////////////////////////////////////////////////////
        public void loadObjFile()
        {
            loadObjFile(File.ReadAllLines(str_objfilename));
        }

	    public void loadObjFile(Stream data)
        {
            using (var reader = new StreamReader(data))
            {
                loadObjFile(reader.ReadToEnd().Split(Environment.NewLine.ToCharArray()));
            }
        }

	    public void loadObjFile(IEnumerable<string> data)
        {
            string localUseMtl;
            
            VAllList = new List<Vertex>();
            VNAllList = new List<VertexNormal>();
            VTAllList = new List<VertexTexture>();
            FAllList = new List<Face>();
            Group g = new Group();

            // init some vars
            localUseMtl = "";

            foreach (var line in data)
            {
                string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length > 0)
                {
                    switch (parts[0])
                    {
                        case "v":
                            Vertex v = new Vertex();
                            v.LoadFromStringArray(parts);
                            VAllList.Add(v);
                            v.Index = VAllList.Count;
                            break;

                        case "vn":
                            VertexNormal vn = new VertexNormal();
                            vn.LoadFromStringArray(parts);
                            VNAllList.Add(vn);
                            vn.Index = VNAllList.Count;
                            break;

                        case "vt":
                            VertexTexture vt = new VertexTexture();
                            vt.LoadFromStringArray(parts);
                            VTAllList.Add(vt);
                            vt.Index = VTAllList.Count;
                            break;

                        case "f":
                            Face tmpF = new Face();

                            tmpF.LoadFromStringArray(parts);
                            tmpF.UseMtl = localUseMtl;

                            tmpF.i_block = g.i_objBlock;
                            tmpF.i_mesh = g.i_objMesh;

                            FAllList.Add(tmpF);
                            break;

                        case "usemtl":
                            localUseMtl = parts[1];
                            break;

                        case "mtllib":
                            str_objmtlfile = parts[1];
                            break;

                        case "g":
                            g.LoadFromStringArray(parts);
                            break;

                        default:
                            break;
                    }                   
                }
            }
        }

        public void processOBJ()
        {
            ST_BLOCK stBlock;
            ST_MESH stMesh;

            SortedList<int, Vertex> VList = new SortedList<int, Vertex>();
            SortedList<int, VertexNormal> VNList = new SortedList<int, VertexNormal>();
            SortedList<int, VertexTexture> VTList = new SortedList<int, VertexTexture>();
            SortedList<int, Face> FList = new SortedList<int, Face>();

            int i_lastBlock, i_lastMesh;
            int i_Vcnt, i_VTcnt, i_Fcnt;

            i_lastBlock = 0;
            i_lastMesh = 0;
            i_VTcnt = 0;
            i_Fcnt = 0;

            //// init main list of blocks and meshes
            stOBJ = new List<ST_BLOCK>();

            //// local Block/Mesh structures
            stBlock = new ST_BLOCK();
            stBlock.stMesh = new List<ST_MESH>();

            // some console text
            Console.Write("\rProcessing Block/Mesh: 00/00");

            // let's mount the OBJ 
            foreach (Face face in FAllList)
            {

                // first check if we are processing other mesh
                if (i_lastMesh != face.i_mesh)
                {
                    // some console text
                    Console.Write("\rProcessing Block/Mesh: " + (face.i_block + 1).ToString("00") + "/" + (face.i_mesh + 1).ToString("00"));

                    stMesh = new ST_MESH();

                    stMesh.VList = new List<Vertex>();
                    stMesh.VList.AddRange(VList.Values);
                    // put keys also as indexes in VList
                    i_Vcnt = 0;
                    foreach (Vertex V in VList.Values) V.Index = VList.Keys[i_Vcnt++];

                    stMesh.VNList = new List<VertexNormal>();
                    stMesh.VNList.AddRange(VNList.Values);

                    stMesh.VTList = new List<VertexTexture>();
                    stMesh.VTList.AddRange(VTList.Values);

                    stMesh.FList = new List<Face>();
                    stMesh.FList.AddRange(FList.Values);

                    // mesh in block's list of meshes
                    stBlock.stMesh.Add(stMesh);

                    // clean lists
                    VList.Clear();
                    VNList.Clear();
                    VTList.Clear();
                    FList.Clear();

                    i_lastMesh = face.i_mesh;


                    if (i_lastBlock != face.i_block)
                    {
                        stOBJ.Add(stBlock);

                        // define a new block/mesh's list
                        stBlock = new ST_BLOCK();
                        stBlock.stMesh = new List<ST_MESH>();

                        i_lastBlock = face.i_block;
                    }

                    i_Fcnt = 0;
                }

                // process Face, putting correctly Vertex, Vertex Normals and Vertex Textures
                for (int i = 0; i < 3; i++)
                {
                    // put Vertices
                    if (VList.ContainsKey(VAllList[face.VertexIndexList[i] - 1].Index) == false)
                        VList.Add(VAllList[face.VertexIndexList[i] - 1].Index,
                                  new Vertex()
                                  {
                                      X = VAllList[face.VertexIndexList[i] - 1].X,
                                      Y = VAllList[face.VertexIndexList[i] - 1].Y,
                                      Z = VAllList[face.VertexIndexList[i] - 1].Z
                                  });

                    // put Vertices Normals if needed
                    if (VNList.ContainsKey(VAllList[face.VertexIndexList[i] - 1].Index) == false)
                        VNList.Add(VAllList[face.VertexIndexList[i] - 1].Index,
                                  new VertexNormal()
                                  {
                                      X = VNAllList[face.VertexNormalIndexList[i] - 1].X,
                                      Y = VNAllList[face.VertexNormalIndexList[i] - 1].Y,
                                      Z = VNAllList[face.VertexNormalIndexList[i] - 1].Z
                                  });

                    // put Vertices Textures if needed (3 vertex)
                    // v0
                    if (VTList.ContainsKey(i_VTcnt + 1) == false)
                        VTList.Add(i_VTcnt + 1,
                                   new VertexTexture()
                                   {
                                       X = VTAllList[face.VertexTextureIndexList[i] - 1].X,
                                       Y = VTAllList[face.VertexTextureIndexList[i] - 1].Y
                                   });

                    i_VTcnt++;
                }

                // put Faces (although they are not needed for .MAP conversion)
                FList.Add(i_Fcnt + 1, face);

                i_Fcnt++;

            }

            // put the last block/mesh into the OBJ structure
            stMesh = new ST_MESH();

            stMesh.VList = new List<Vertex>();
            stMesh.VList.AddRange(VList.Values);
            // put keys also as indexes in VList
            i_Vcnt = 0;
            foreach (Vertex V in VList.Values) V.Index = VList.Keys[i_Vcnt++];

            stMesh.VNList = new List<VertexNormal>();
            stMesh.VNList.AddRange(VNList.Values);

            stMesh.VTList = new List<VertexTexture>();
            stMesh.VTList.AddRange(VTList.Values);

            stMesh.FList = new List<Face>();
            stMesh.FList.AddRange(FList.Values);

            // mesh in block's list of meshes
            stBlock.stMesh.Add(stMesh);
            stOBJ.Add(stBlock);

            Console.WriteLine("\r");
        }
    }
}

