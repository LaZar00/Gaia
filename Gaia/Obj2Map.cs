using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using static Gaia.Globals;
using static Gaia.Materials;
using static Gaia.Walkmap;
using static Gaia.Lzs;

namespace Gaia
{
    public class Obj2Map
    {
 
        OBJ obj2map;
        public int i_objnblocks;

        public Obj2Map()
        {
            obj2map = new OBJ();

            try
            {
                // load .obj file
                Console.WriteLine("Reading OBJ file: " + str_objfilename + "...");
                obj2map.loadObjFile();
            }
            catch
            {
                throw new ArgumentException("Error reading OBJ file.");
            }

            try
            {
                // process .obj data
                Console.WriteLine("Processing OBJ data...");
                obj2map.processOBJ();
                i_objnblocks = obj2map.stOBJ.Count;
            }
            catch
            {
                throw new ArgumentException("Error processing OBJ data.");
            }
        }


        // some custom Exceptions
        [Serializable]
        class BlockSizeExceeded: Exception
        {
            public BlockSizeExceeded() { }

            public BlockSizeExceeded(int i_block, int i_blcksz)
                : base(String.Format("Block " + i_block.ToString("00") + " exceeds the 47104 (0xB800) bytes in: " +
                                     (i_blcksz - 0xB800).ToString("00000") + "/0x" +
                                     (i_blcksz - 0xB800).ToString("X") + " bytes.")) { }
        }

        [Serializable]
        class ErrorWithMaterial : Exception
        {
            public ErrorWithMaterial() { }

            public ErrorWithMaterial(string str_material)
                : base(String.Format("There has been some problem with the material: '" +
                                     str_material + "' of this OBJ file.")) { }
        }

        [Serializable]
        class MaterialNotFound : Exception
        {
            public MaterialNotFound() { }

            public MaterialNotFound(string str_material)
                : base(String.Format("Material not found in .mtl file when doing the OBJ to MAP process.")) { }
        }
        


        // in this function we will create the .MAP structure from the .OBJ structure
        // we have all the data, so, we will need only to adapt some calculations
        // at this moment, we know the number of blocks, vertices, normals, vertex textures...
        public int export2Map()
        {
            int i_result = 0;
            int i_block, i_mesh, i_vertex, i_triangle, i_VTcnt;
            int row_Block, col_Block, row_Mesh, col_Mesh;
            double v0, v1, v2, ut0, vt0, ut1, vt1, ut2, vt2;
            double i_texwidth, i_texheight, i_texuoffset, i_texvoffset;
            string[] str_matNameSplit;
            string str_matName;
            int i_matIdxID;

            UInt16 i_nvertex, i_ntriangles;

            // assign global number of blocks for new .MAP file and initialize wm_block
            i_nblocks = obj2map.stOBJ.Count;
            initialize_wmblock();

            // some info
            Console.WriteLine("Converting internal .OBJ data to .MAP data...");

            try
            {
                // populate the MAP structure
                // for each block
                for (i_block = 0; i_block < i_nblocks; i_block++)
                {

                    // let's get the col/row block we are treating
                    col_Block = i_block % i_ncols;
                    row_Block = i_block / i_ncols;

                    // for each mesh
                    for (i_mesh = 0; i_mesh < N_MESHES; i_mesh++)
                    {
                        // some info
                        Console.Write("\rProcessing Block/Mesh: " + (i_block + 1).ToString("00") + "/" + (i_mesh + 1).ToString("00"));

                        // let's get the col/row mesh we are treating
                        col_Mesh = i_mesh % 4;
                        row_Mesh = i_mesh / 4;

                        i_nvertex = (UInt16)obj2map.stOBJ[i_block].stMesh[i_mesh].VList.Count;
                        i_ntriangles = (UInt16)obj2map.stOBJ[i_block].stMesh[i_mesh].FList.Count;

                        wm_block[i_block].wm_mesh[i_mesh].i_numVertices = i_nvertex;
                        wm_block[i_block].wm_mesh[i_mesh].i_numTriangles = i_ntriangles;

                        wm_block[i_block].wm_mesh[i_mesh].wm_vertices = new WM_MESH_VERTEX[i_nvertex];
                        wm_block[i_block].wm_mesh[i_mesh].wm_triangles = new WM_MESH_TRIANGLE[i_ntriangles];


                        // let's put vertices                    // 
                        for (i_vertex = 0; i_vertex < i_nvertex; i_vertex++)
                        {
                            wm_block[i_block].wm_mesh[i_mesh].wm_vertices[i_vertex].x =
                                    (Int16)(obj2map.stOBJ[i_block].stMesh[i_mesh].VList[i_vertex].X -
                                           (col_Block * BLOCK_PIXELS) -
                                           (col_Mesh * MESH_PIXELS));

                            wm_block[i_block].wm_mesh[i_mesh].wm_vertices[i_vertex].y =
                                    (Int16)obj2map.stOBJ[i_block].stMesh[i_mesh].VList[i_vertex].Y;

                            wm_block[i_block].wm_mesh[i_mesh].wm_vertices[i_vertex].z =
                                    (Int16)(obj2map.stOBJ[i_block].stMesh[i_mesh].VList[i_vertex].Z -
                                           (row_Block * BLOCK_PIXELS) -
                                           (row_Mesh * MESH_PIXELS));
                        }

                        // let's put vertex normals
                        for (i_vertex = 0; i_vertex < i_nvertex; i_vertex++)
                        {
                            wm_block[i_block].wm_mesh[i_mesh].wm_vertices[i_vertex].xn =
                                    (Int16)Math.Round(obj2map.stOBJ[i_block].stMesh[i_mesh].VNList[i_vertex].X * VERTEX_NORMAL_DEF);
                            //(Int16)(obj2map.stOBJ[i_block].stMesh[i_mesh].VNList[i_vertex].X * VERTEX_NORMAL_DEF);

                            wm_block[i_block].wm_mesh[i_mesh].wm_vertices[i_vertex].yn =
                                    (Int16)Math.Round(obj2map.stOBJ[i_block].stMesh[i_mesh].VNList[i_vertex].Y * VERTEX_NORMAL_DEF);
                            //(Int16)(obj2map.stOBJ[i_block].stMesh[i_mesh].VNList[i_vertex].Y * VERTEX_NORMAL_DEF);

                            wm_block[i_block].wm_mesh[i_mesh].wm_vertices[i_vertex].zn =
                                    (Int16)Math.Round(obj2map.stOBJ[i_block].stMesh[i_mesh].VNList[i_vertex].Z * VERTEX_NORMAL_DEF);
                            //(Int16)(obj2map.stOBJ[i_block].stMesh[i_mesh].VNList[i_vertex].Z * VERTEX_NORMAL_DEF);
                        }

                        // write uvmap
                        i_VTcnt = 0;

                        for (i_triangle = 0; i_triangle < wm_block[i_block].wm_mesh[i_mesh].i_numTriangles; i_triangle++)
                        {
                            // first we will put material id and special bits values
                            // first, let's get the material Name
                            str_matNameSplit = (obj2map.stOBJ[i_block].stMesh[i_mesh].FList[i_triangle].UseMtl).Split('_');

                            // let's check if material is correct. If not throw exception.
                            if (str_matNameSplit.Length < 4)
                                throw new ErrorWithMaterial(obj2map.stOBJ[i_block].stMesh[i_mesh].FList[i_triangle].UseMtl);

                            str_matName = str_matNameSplit[0];
                            wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].textureID = Int32.Parse(str_matNameSplit[str_matNameSplit.Length - 5]);

                            // check if material is in internal table
                            if (wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].textureID >= stMaterialID.Count)
                                throw new MaterialNotFound("Material not found in .mtl file when doing the OBJ to MAP process.");
                               

                            i_matIdxID = wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].textureID;

                            // get texture width/height/uvoffsets
                            i_texwidth = stMaterialID[i_matIdxID].MWidth;
                            i_texheight = stMaterialID[i_matIdxID].MHeight;
                            i_texuoffset = stMaterialID[i_matIdxID].UOffset;
                            i_texvoffset = stMaterialID[i_matIdxID].VOffset;

                            wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].locationID = Int32.Parse(str_matNameSplit[str_matNameSplit.Length - 4]);
                            wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].walkmapID = Int32.Parse(str_matNameSplit[str_matNameSplit.Length - 2]);
                            wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].walkmapUNK = Int32.Parse(str_matNameSplit[str_matNameSplit.Length - 1]);

                            wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].walkmap_unk =
                                Convert.ToByte((wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].walkmapID & 0x1F) |
                                               (wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].walkmapUNK << 5));

                            wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].texture_location =
                                      (UInt16)(wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].textureID & 0x1FF |
                                               wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].locationID << 9);


                            // second, we will process the v, vn, vts values of the triangle
                            // put uv indexed triangle vertices
                            v0 = obj2map.stOBJ[i_block].stMesh[i_mesh].VList.FindIndex(
                                    x => x.Index == obj2map.stOBJ[i_block].stMesh[i_mesh].FList[i_triangle].VertexIndexList[0]);
                            v1 = obj2map.stOBJ[i_block].stMesh[i_mesh].VList.FindIndex(
                                    x => x.Index == obj2map.stOBJ[i_block].stMesh[i_mesh].FList[i_triangle].VertexIndexList[1]);
                            v2 = obj2map.stOBJ[i_block].stMesh[i_mesh].VList.FindIndex(
                                    x => x.Index == obj2map.stOBJ[i_block].stMesh[i_mesh].FList[i_triangle].VertexIndexList[2]);

                            // prepare utexture vertices
                            ut0 = Math.Round(obj2map.stOBJ[i_block].stMesh[i_mesh].VTList[i_VTcnt].X * i_texwidth) + i_texuoffset;
                            ut1 = Math.Round(obj2map.stOBJ[i_block].stMesh[i_mesh].VTList[i_VTcnt + 1].X * i_texwidth) + i_texuoffset;
                            ut2 = Math.Round(obj2map.stOBJ[i_block].stMesh[i_mesh].VTList[i_VTcnt + 2].X * i_texwidth) + i_texuoffset;

                            // prepare vtexture vertices
                            vt0 = obj2map.stOBJ[i_block].stMesh[i_mesh].VTList[i_VTcnt].Y;
                            if (vt0 > 1) vt0 = (vt0 - 1) * -1;
                            else vt0 = 1 - vt0;
                            vt0 = Math.Round((vt0 * i_texheight) + i_texvoffset);

                            vt1 = obj2map.stOBJ[i_block].stMesh[i_mesh].VTList[i_VTcnt + 1].Y;
                            if (vt1 > 1) vt1 = (vt1 - 1) * -1;
                            else vt1 = 1 - vt1;
                            vt1 = Math.Round((vt1 * i_texheight) + i_texvoffset);

                            vt2 = obj2map.stOBJ[i_block].stMesh[i_mesh].VTList[i_VTcnt + 2].Y;
                            if (vt2 > 1) vt2 = (vt2 - 1) * -1;
                            else vt2 = 1 - vt2;
                            vt2 = Math.Round((vt2 * i_texheight) + i_texvoffset);


                            // assign triangle values to array vars
                            wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].v0 = (byte)v0;
                            wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].v1 = (byte)v1;
                            wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].v2 = (byte)v2;

                            wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].ut0 = (byte)ut0;
                            wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].ut1 = (byte)ut1;
                            wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].ut2 = (byte)ut2;

                            wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].vt0 = (byte)vt0;
                            wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].vt1 = (byte)vt1;
                            wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].vt2 = (byte)vt2;

                            i_VTcnt += 3;
                        }
                    }
                }
            }
            catch (ErrorWithMaterial ex)
            {
                i_result = -1;
                Console.WriteLine("\r");
                Console.WriteLine(ex.Message);
            }
            catch (MaterialNotFound ex)
            {
                i_result = -1;
                Console.WriteLine("\r");
                Console.WriteLine(ex.Message);
            }
            catch
            {
                i_result = -1;
            }

            Console.WriteLine("\r");

            // now we need to create the .lzs uncompressed data for each mesh
            i_result = generate_Meshbinarydata();


            // finally we save the worldmap memory data in the MAP file
            // or dump the files if asked WITHOUT writing the MAP file
            if (b_dumpdata) i_result = dumpBinaryData(i_nblocks);
            else
            {
                i_result = writeMAP();

                if (i_result == 0)
                {
                    // init vars for make the rebuild here
                    b_rebuild = true;
                    str_mapfilename = str_filename + ".MAP";

                    Map2Bot wm_bot = new Map2Bot();

                    i_result = wm_bot.rebuildBOT();

                    if (i_result == 0)
                        Console.WriteLine("Rebuild process of .BOT file from .MAP file... FINISHED.");
                    else
                        Console.WriteLine("Exception Error when rebuilding the .BOT file...");

                }

            }

            return i_result;
        }


        //
        public int generate_Meshbinarydata()
        {
            int i_result = 0;
            int i_block, i_mesh, i_triangle, i_vertex;
            int sz_TotalOffset, i_padding, lsh_pad;
            MemoryStream outputCompMesh;

            try
            {
                // some text
                Console.WriteLine("Generating Binary MAP data and compressing meshes in LZS format...");

                // for each block
                for (i_block = 0; i_block < i_nblocks; i_block++)
                {
                    // init array of meshes in raw format
                    wm_block[i_block].offset = new int[N_MESHES];
                    wm_block[i_block].wm_mesh_raw = new WM_MESH_RAW[N_MESHES];

                    // for each mesh
                    // init offsets for the block of meshes
                    sz_TotalOffset = 0x40;

                    for (i_mesh = 0; i_mesh < N_MESHES; i_mesh++)
                    {
                        // here we will create the binary uncompressed mesh
                        // we will treat memory array as a memorystream

                        using (MemoryStream ms_Mesh = new MemoryStream())
                        {
                            using (BinaryWriter bw_Mesh = new BinaryWriter(ms_Mesh))
                            {
                                // write num triangles / vertices
                                bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].i_numTriangles);
                                bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].i_numVertices);

                                // write triangles data
                                for (i_triangle = 0; i_triangle < wm_block[i_block].wm_mesh[i_mesh].i_numTriangles; i_triangle++)
                                {
                                    bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].v0);
                                    bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].v1);
                                    bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].v2);

                                    bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].walkmap_unk);
                                    bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].ut0);
                                    bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].vt0);
                                    bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].ut1);
                                    bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].vt1);
                                    bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].ut2);
                                    bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].vt2);
                                    bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].texture_location);
                                }

                                // write vertex data
                                for (i_vertex = 0; i_vertex < wm_block[i_block].wm_mesh[i_mesh].i_numVertices; i_vertex++)
                                {
                                    bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].wm_vertices[i_vertex].x);
                                    bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].wm_vertices[i_vertex].y);
                                    bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].wm_vertices[i_vertex].z);
                                    bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].wm_vertices[i_vertex].w);
                                }

                                // write vertex normal data
                                for (i_vertex = 0; i_vertex < wm_block[i_block].wm_mesh[i_mesh].i_numVertices; i_vertex++)
                                {
                                    bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].wm_vertices[i_vertex].xn);
                                    bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].wm_vertices[i_vertex].yn);
                                    bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].wm_vertices[i_vertex].zn);
                                    bw_Mesh.Write(wm_block[i_block].wm_mesh[i_mesh].wm_vertices[i_vertex].wn);
                                }

                            }

                            // put memorystream in the byte array
                            wm_block[i_block].wm_mesh_raw[i_mesh].uncomp_lzs_mesh = ms_Mesh.ToArray();

                            // HERE WE COMPRESS THE MESH IN LZS
                            outputCompMesh = new MemoryStream();
                            Encode(new MemoryStream(wm_block[i_block].wm_mesh_raw[i_mesh].uncomp_lzs_mesh), outputCompMesh);
                            wm_block[i_block].wm_mesh_raw[i_mesh].comp_lzs_mesh = outputCompMesh.ToArray();
                        }

                        // we will calculate the offsets for the meshes of each block
                        // this will make easy the saving later
                        // hdr: list of 16 offsets to the meshes (1 for each mesh) padded to 4bits
                        // data: <size of mesh compressed -4 bytes-><mesh compressed byte array>
                        // when 16 meshes done, fill the data until 0xB800 bytes filled in the block
                        wm_block[i_block].offset[i_mesh] = sz_TotalOffset;

                        // add padding to the offset. we need to add the padding with zeroes to the end of lzs file
                        lsh_pad = wm_block[i_block].wm_mesh_raw[i_mesh].comp_lzs_mesh.Length & 0b11;
                        if (lsh_pad == 0) i_padding = 0;
                        else i_padding = 4 - lsh_pad;
                        sz_TotalOffset = sz_TotalOffset + 4 + wm_block[i_block].wm_mesh_raw[i_mesh].comp_lzs_mesh.Length + i_padding;
                    }
                }

            }
            catch
            {
                i_result = -1;
            }        

            return i_result;
        }


        public int writeMAP()
        {
            int i_result = 0;
            int i_block, i_mesh;
            int sz_Block, i_padding;

            try
            {
                // some text
                Console.WriteLine("Writing MAP file...");

                using (BinaryWriter bw_MAP = new BinaryWriter(File.Open(str_filename + ".MAP", FileMode.Create)))
                {
                    for (i_block = 0; i_block < i_nblocks; i_block++)
                    {
                        // for each block
                        sz_Block = 0;

                        // write offsets to meshes of 'n' i_block
                        for (i_mesh = 0; i_mesh < N_MESHES; i_mesh++)
                        {
                            bw_MAP.Write(wm_block[i_block].offset[i_mesh]);
                        }
                        // count bytes to the size of block
                        sz_Block = 0x40;

                        // write meshes
                        // write size + data and padding if needed
                        for (i_mesh = 0; i_mesh < N_MESHES; i_mesh++)
                        {
                            // write MAP size
                            bw_MAP.Write(wm_block[i_block].wm_mesh_raw[i_mesh].comp_lzs_mesh.Length);

                            // write MAP data
                            bw_MAP.Write(wm_block[i_block].wm_mesh_raw[i_mesh].comp_lzs_mesh);

                            // write padding if needed for each mesh to 4bits.
                            int lsh_pad = wm_block[i_block].wm_mesh_raw[i_mesh].comp_lzs_mesh.Length & 0b11;
                            i_padding = 4 - lsh_pad;
                            if (i_padding == 4) i_padding = 0;
                            if (i_padding > 0)
                                bw_MAP.Write(new byte[i_padding]);

                            // count bytes of block
                            sz_Block = sz_Block + 4 + wm_block[i_block].wm_mesh_raw[i_mesh].comp_lzs_mesh.Length + i_padding;
                        }

                        // we can check if sz_Block is greater than 0xB800 or not
                        if (sz_Block > 0xB800)
                            throw new BlockSizeExceeded(i_block, sz_Block);

                        bw_MAP.Write(new byte[0xB800 - sz_Block]);

                    }

                }
            }
            catch (BlockSizeExceeded ex)
            {
                i_result = -2;
                Console.WriteLine(ex.Message);
            }
            catch
            {
                i_result = -1;
            }

            return i_result;
        }


    }
}
