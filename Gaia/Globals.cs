using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace Gaia
{
    public class Globals
    {

        public const int BLOCK_SIZE = 0xB800;
        public const int BLOCK_PIXELS = 32768;
        public const int MESH_PIXELS = 8192;
        public const double VERTEX_NORMAL_DEF = 4096;
        public const int N_MESHES = 16;
        public const int N_WM0COLS = 9;
        public const int N_WM2COLS = 3;
        public const int N_WM3COLS = 2;
        public const int N_WM0NORMALBLOCKS = 63;
        public const int N_WM2NORMALBLOCKS = 12;
        public const int N_WM3NORMALBLOCKS = 4;
        public const int N_BLOCKSBOT = 332;


        // WORLDMAP BLOCK
        // The .MAP file consist on 'n' Blocks with a size of 0xB800 bytes each one.
        // Each Block has a size of 32768x32768 coordinate points.
        // You can find the number of blocks by diving the file size with the block size.
        // Then each Block consists in a grid of 4x4 MESHES (8192x8192 coordinate points).
        public struct WM_MESH_RAW
        {
            public int lzs_mesh_size;   // mesh size is lsz length
            public byte[] comp_lzs_mesh;
            public byte[] uncomp_lzs_mesh;
        }

        public struct WM_BLOCK
        {
            public int[] offset;
            public WM_MESH_RAW[] wm_mesh_raw;
            public WM_MESH[] wm_mesh;
        }


        // BLOCK MESH
        public struct WM_MESH
        {
            public UInt16 i_numTriangles;
            public UInt16 i_numVertices;
            public WM_MESH_TRIANGLE[] wm_triangles;
            public WM_MESH_VERTEX[] wm_vertices;
        }

        public struct WM_MESH_TRIANGLE
        {
            public byte v0;
            public byte v1;
            public byte v2;
            public byte walkmap_unk;
            public byte ut0;
            public byte vt0;
            public byte ut1;
            public byte vt1;
            public byte ut2;
            public byte vt2;
            public UInt16 texture_location;
            public int walkmapID;
            public int walkmapUNK;
            public int textureID;
            public int locationID;
        }

        public struct WM_MESH_VERTEX
        {
            public Int16 x;
            public Int16 y;
            public Int16 z;
            public UInt16 w; // always 0?
            public Int16 xn;
            public Int16 yn;
            public Int16 zn;
            public UInt16 wn;  // always 0?
        }

        // global vars
        public static string str_filename, str_fileext, str_mapfilename, str_objfilename, str_mtlfilename, str_botfilename;
        public static string str_imgformat, str_texturepath, str_blockspath, str_dumpspath;
        public static bool b_fullmaterial, b_byblocks, b_dumpdata, b_isbot, b_rebuild;


        // in the Material names of the .mtl file.
        public static WM_BLOCK[] wm_block;
        public static WM_BLOCK[] wm_blockbot;

        public static int i_nblocks { get; set; }
        public static int i_nmaxrebuildblocks { get; set; }
        public static int i_nblocksbot { get; set; }
        public static int i_ncols { get; set; }
        public static int i_nrows { get; set; }


        public static void get_nblocksmapfile()
        {
            int i_filesize;

            // Let's tell the number of blocks of the map to the program
            i_filesize = (int)new FileInfo(str_mapfilename).Length;

            if (i_filesize % BLOCK_SIZE == 0)
                i_nblocks = (int)new FileInfo(str_mapfilename).Length / BLOCK_SIZE;
        }


        public static void get_nblocksbotfile()
        {
            // Let's tell the number of blocks of the bot to the program
            i_nblocksbot = N_BLOCKSBOT;
        }

        //public static void get_nblocksobjfile(string[] str_objlines)
        //{
        //    int i_objcount;

        //    i_objcount = (from objline in str_objlines
        //                  where objline.StartsWith("o")
        //                  select objline).Count();

        //    i_nblocks = i_objcount / N_MESHES;
        //}

        public static void initialize_wmblock()
        {
            int i_block;

            wm_block = new WM_BLOCK[i_nblocks];

            for (i_block = 0; i_block < i_nblocks; i_block++)
            {
                wm_block[i_block].wm_mesh = new WM_MESH[N_MESHES];
            }

            get_ncols();
        }

        public static void get_ncols()
        {
            if (i_nblocks > 12)
            {
                i_nmaxrebuildblocks = N_WM0NORMALBLOCKS;
                i_ncols = N_WM0COLS;
                i_nrows = N_WM0NORMALBLOCKS / N_WM0COLS;
            }
            else if (i_nblocks == 12)
            {
                i_nmaxrebuildblocks = N_WM2NORMALBLOCKS;
                i_ncols = N_WM2COLS;
                i_nrows = N_WM2NORMALBLOCKS / N_WM2COLS;
            }
            else
            {
                i_nmaxrebuildblocks = N_WM3NORMALBLOCKS;
                i_ncols = N_WM3COLS;
                i_nrows = N_WM3NORMALBLOCKS / N_WM3COLS;
            }
        }

        public static void initialize_wmblockbot()
        {
            int i_block;

            wm_blockbot = new WM_BLOCK[i_nblocksbot];

            for (i_block = 0; i_block < i_nblocksbot; i_block++)
            {
                wm_blockbot[i_block].wm_mesh = new WM_MESH[N_MESHES];
            }
        }

        public static int dumpBinaryData(int i_dumpnblocks)
        {
            int i_result;
            int i_block, i_mesh;

            i_result = 0;

            try
            {
                // some text output
                Console.WriteLine("Writing Uncompressed Binary Dumps of the Blocks/Meshes of .MAP/.BOT/.OBJ file...");

                for (i_block = 0; i_block < i_dumpnblocks; i_block++)
                {
                    for (i_mesh = 0; i_mesh < N_MESHES; i_mesh++)
                    {
                        // some console text
                        Console.Write("\rWriting Block: " + (i_block + 1).ToString("00") + "/" + i_nblocks.ToString("00") +
                                                ", Mesh: " + (i_mesh + 1).ToString("00") + "/" + N_MESHES.ToString("00") + "...");

                        File.WriteAllBytes(str_dumpspath + "\\" + "Block" + i_block.ToString("00") +
                                                                  "_Mesh" + i_mesh.ToString("00") + ".dec",
                                           wm_block[i_block].wm_mesh_raw[i_mesh].uncomp_lzs_mesh);

                    }
                }

                Console.WriteLine("\r");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("The directory for output the dumps doesn't exists.");
                i_result = -2;
            }
            catch
            {
                i_result = -1;
            }

            return i_result;
        }
    }
}
