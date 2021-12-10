using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using static Gaia.Globals;
using static Gaia.Lzs;


namespace Gaia
{

    public class Map2Bot
    {

        // This static vars will help in the Block materials output including internal data of the field
        // They can be accessed from other classes.
        public static OBJ objOutput;
        public static HashSet<string>[] lst_MatBlock;


        public Map2Bot()
        {
            int i_result;

            // if we create the bot for rebuild process, there is no need to process BOT and fill the meshes
            if (!b_rebuild)
            {
                Console.WriteLine("Reading BOT file: " + str_botfilename + "...");
                i_result = loadBotFile();


                // now we will populate the meshes in memory
                // let's output some text
                if (i_result == 0)
                {
                    Console.WriteLine("Processing BOT Meshes...");
                    i_result = processBOT();

                    if (i_result == -1)
                    {
                        Console.WriteLine("Exception Error while processing .BOT file '" + str_botfilename + "'.\r\n");
                        Environment.Exit(0);
                    }
                }
                else if (i_result == -1)
                {
                    Console.WriteLine("Exception Error while loading .BOT file '" + str_botfilename + "'.\r\n");
                    Environment.Exit(0);
                }
                else if (i_result == -2)
                {
                    Console.WriteLine("Error: File '" + str_botfilename + "' has wrong .BOT format.\r\n");
                    Environment.Exit(0);
                }
            }
        }


        //// some custom Exceptions
        //[Serializable]
        //class BlockSizeExceeded : Exception
        //{
        //    public BlockSizeExceeded() { }

        //    public BlockSizeExceeded(int i_block, int i_blcksz)
        //        : base(String.Format("Block " + i_block.ToString("00") + " exceeds the 47104 (0xB800) bytes in: " +
        //                             (i_blcksz - 0xB800).ToString("00000") + "/0x" +
        //                             (i_blcksz - 0xB800).ToString("X") + " bytes."))
        //    { }
        //}


        public int loadBotFile()
        {
            int i_result;
            int i_block, i_mesh;
            MemoryStream outputUncompMesh;

            i_result = 0;

            // init some things

            // first let's prepare number of blocks and determine number of columns
            // we can initialize also the wm_block structure
            get_nblocksbotfile();
            initialize_wmblockbot();

            // check if the file has the correct block number
            if (i_nblocksbot > 0)
            {
                // load/read .bot blocks into memory
                try
                {

                    using (BinaryReader mapReader = new BinaryReader(File.Open(str_botfilename, FileMode.Open)))
                    {
                        // read nblocks
                        for (i_block = 0; i_block < i_nblocksbot; i_block++)
                        {

                            // init wm_block
                            wm_blockbot[i_block].offset = new int[N_MESHES];
                            wm_blockbot[i_block].wm_mesh_raw = new WM_MESH_RAW[N_MESHES];

                            // position reader to next block
                            mapReader.BaseStream.Seek(i_block * BLOCK_SIZE, SeekOrigin.Begin);

                            // read 16 offsets to meshes
                            for (i_mesh = 0; i_mesh < N_MESHES; i_mesh++)
                            {
                                wm_blockbot[i_block].offset[i_mesh] = mapReader.ReadInt32();
                            }

                            // read 16 meshes/block (in lzs format)
                            for (i_mesh = 0; i_mesh < N_MESHES; i_mesh++)
                            {
                                // Some output text
                                Console.Write("\rReading Block/Mesh: " + (i_block + 1).ToString("00") + "/" + (i_mesh + 1).ToString("00") + "        ");

                                // get lzs mesh size
                                mapReader.BaseStream.Seek((i_block * BLOCK_SIZE) + wm_blockbot[i_block].offset[i_mesh], SeekOrigin.Begin);
                                wm_blockbot[i_block].wm_mesh_raw[i_mesh].lzs_mesh_size = mapReader.ReadInt32();

                                // put lzs mesh in memory
                                mapReader.BaseStream.Seek((i_block * BLOCK_SIZE) + wm_blockbot[i_block].offset[i_mesh] + 4, SeekOrigin.Begin);
                                wm_blockbot[i_block].wm_mesh_raw[i_mesh].comp_lzs_mesh = new byte[wm_blockbot[i_block].wm_mesh_raw[i_mesh].lzs_mesh_size];
                                wm_blockbot[i_block].wm_mesh_raw[i_mesh].comp_lzs_mesh =
                                                    mapReader.ReadBytes((int)wm_blockbot[i_block].wm_mesh_raw[i_mesh].lzs_mesh_size);

                                // HERE WE UNCOMPRESS LZS
                                outputUncompMesh = new MemoryStream();
                                Decode(new MemoryStream(wm_blockbot[i_block].wm_mesh_raw[i_mesh].comp_lzs_mesh), outputUncompMesh);

                                // put uncompressed mesh in structure
                                wm_blockbot[i_block].wm_mesh_raw[i_mesh].uncomp_lzs_mesh = outputUncompMesh.ToArray();
                            }
                        }
                    }

                    Console.Write("\r\n");
                }
                catch
                {
                    i_result = -1;
                }
            }
            else
            {
                i_result = -2;
            }

            return i_result;
        }


        public int processBOT()
        {
            int i_result;
            int i_block, i_mesh, i_tri;

            // let's populate the 3D Data of each Block (meshes) into structs
            // we have the data uncompressed in memory
            i_result = 0;

            // populate each block
            try
            {
                for (i_block = 0; i_block < i_nblocksbot; i_block++)
                {
                    // populate each mesh
                    for (i_mesh = 0; i_mesh < N_MESHES; i_mesh++)
                    {
                        using (MemoryStream ms_meshraw = new MemoryStream(wm_blockbot[i_block].wm_mesh_raw[i_mesh].uncomp_lzs_mesh))
                        {
                            using (BinaryReader br_meshraw = new BinaryReader(ms_meshraw))
                            {
                                // mesh header
                                wm_blockbot[i_block].wm_mesh[i_mesh].i_numTriangles = br_meshraw.ReadUInt16();
                                wm_blockbot[i_block].wm_mesh[i_mesh].i_numVertices = br_meshraw.ReadUInt16();

                                // memory for mesh data
                                wm_blockbot[i_block].wm_mesh[i_mesh].wm_triangles = new WM_MESH_TRIANGLE[wm_blockbot[i_block].wm_mesh[i_mesh].i_numTriangles];
                                wm_blockbot[i_block].wm_mesh[i_mesh].wm_vertices = new WM_MESH_VERTEX[wm_blockbot[i_block].wm_mesh[i_mesh].i_numVertices];

                                // read triangles
                                for (i_tri = 0; i_tri < wm_blockbot[i_block].wm_mesh[i_mesh].i_numTriangles; i_tri++)
                                {
                                    // file data
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_triangles[i_tri].v0 = br_meshraw.ReadByte();
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_triangles[i_tri].v1 = br_meshraw.ReadByte();
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_triangles[i_tri].v2 = br_meshraw.ReadByte();
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_triangles[i_tri].walkmap_unk = br_meshraw.ReadByte();
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_triangles[i_tri].ut0 = br_meshraw.ReadByte();
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_triangles[i_tri].vt0 = br_meshraw.ReadByte();
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_triangles[i_tri].ut1 = br_meshraw.ReadByte();
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_triangles[i_tri].vt1 = br_meshraw.ReadByte();
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_triangles[i_tri].ut2 = br_meshraw.ReadByte();
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_triangles[i_tri].vt2 = br_meshraw.ReadByte();
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_triangles[i_tri].texture_location = br_meshraw.ReadUInt16();

                                    // bit conversion special data
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_triangles[i_tri].walkmapID =
                                                wm_blockbot[i_block].wm_mesh[i_mesh].wm_triangles[i_tri].walkmap_unk & 0x1F;
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_triangles[i_tri].walkmapUNK =
                                                wm_blockbot[i_block].wm_mesh[i_mesh].wm_triangles[i_tri].walkmap_unk >> 5;

                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_triangles[i_tri].textureID =
                                                wm_blockbot[i_block].wm_mesh[i_mesh].wm_triangles[i_tri].texture_location & 0x1FF;
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_triangles[i_tri].locationID =
                                                wm_blockbot[i_block].wm_mesh[i_mesh].wm_triangles[i_tri].texture_location >> 9;
                                }

                                // read vertices
                                for (i_tri = 0; i_tri < wm_blockbot[i_block].wm_mesh[i_mesh].i_numVertices; i_tri++)
                                {
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_vertices[i_tri].x = br_meshraw.ReadInt16();
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_vertices[i_tri].y = br_meshraw.ReadInt16();
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_vertices[i_tri].z = br_meshraw.ReadInt16();
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_vertices[i_tri].w = br_meshraw.ReadUInt16();
                                }

                                // read normals
                                for (i_tri = 0; i_tri < wm_blockbot[i_block].wm_mesh[i_mesh].i_numVertices; i_tri++)
                                {
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_vertices[i_tri].xn = br_meshraw.ReadInt16();
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_vertices[i_tri].yn = br_meshraw.ReadInt16();
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_vertices[i_tri].zn = br_meshraw.ReadInt16();
                                    wm_blockbot[i_block].wm_mesh[i_mesh].wm_vertices[i_tri].wn = br_meshraw.ReadUInt16();
                                }
                            }
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


        // function to rebuild the .BOT file from .MAP file
        // in this case we don't need to process the data, only load Blocks and arrange them in the .BOT file
        public int rebuildBOT()
        {
            int i_result;
            int i_block, i_blockBotArrange;
            int i_srcBlock;

            List<byte[]> lst_MAPBlocks, lst_BOTBlocks;

            // init some vars
            lst_MAPBlocks = new List<byte[]>();

            // first let's prepare number of blocks and determine number of columns
            // prepare some vars of Globasl (i_nblocks, i_ncols, i_nrows...)
            get_nblocksmapfile();
            get_ncols();


            // some text
            i_result = loadBlocks(lst_MAPBlocks);

            if (i_result == 0)
            {
                // some text
                Console.WriteLine("Rebuilding BOT file: " + str_botfilename + "...");

                // The resulting .BOT file is compressed of 332 Blocks (WM0)
                // The structure of each block of the .BOT is described in the text file 'fundamentals\BOTstructure'.
                // Basically, the .BOT file has a defined structure for each main block of .MAP file.
                // When using Block 0 of .MAP file, the .BOT file creates Blocks 0, 1, 9, 10 as the first 4 Blocks (Blocks numbers 0,1,2,3).
                // When using Block 1 of .MAP file, the .BOT file creates Blocks 1, 2, 10, 11 as the second 4 Blocks (Blocks numbers 4,5,6,7).
                // ...
                // When using Block 8 of .MAP file, the .BOT file creates Blocks 8, 0, 17, 9  as the nineth 4 Blocks (Blocks numbers 32, 33, 34, 35).
                // ...and so on with the next row. The latest row of the Blocks (0-62) uses again the first row of Blocks as arranged in the .MAP.
                // We have also the special cases for blocks 63-68, they are also described in the text file.

                // First we will create the first 252 Blocks in 4 Blocks arrangements.
                // We will do this process in memory.
                // We only need to work with blocks, definitely.
                try
                {
                    // init list of BOT Blocks
                    lst_BOTBlocks = new List<byte[]>();

                    // if we are working with WM0 we need to add the special blocks
                    if (i_ncols > 3)
                    {
                        for (i_block = 0; i_block < i_nmaxrebuildblocks; i_block++)
                        {
                            // we must separate the normal blocks of a worldmap with the special blocks
                            // (0-62) 63 normal blocks
                            // now we need to put this .MAP block in .BOT structure
                            // each block of .MAP is arranged in 4xblocks arrangement in .BOT
                            for (i_blockBotArrange = 0; i_blockBotArrange < 4; i_blockBotArrange++)
                            {
                                i_srcBlock = i_block;

                                // calculate the source Block of .MAP file as per .BOT structure
                                switch (i_blockBotArrange)
                                {
                                    case 1:
                                        i_srcBlock = i_block + 1;

                                        if (i_srcBlock % i_ncols == 0)
                                            i_srcBlock -= i_ncols;

                                        break;

                                    case 2:
                                        i_srcBlock = i_block + i_ncols;

                                        if (i_srcBlock >= i_nmaxrebuildblocks)
                                            i_srcBlock -= i_nmaxrebuildblocks;
                                        break;

                                    case 3:
                                        i_srcBlock = i_block + i_ncols + 1;

                                        if (i_srcBlock >= i_nmaxrebuildblocks)
                                            i_srcBlock -= i_nmaxrebuildblocks;
                                        else
                                            if (i_srcBlock % i_ncols == 0)
                                            i_srcBlock -= i_ncols;

                                        if (i_srcBlock == i_nmaxrebuildblocks) i_srcBlock = 0;
                                        break;
                                }


                                // put the block of .MAP in .BOT structure
                                // for this we will work directly with .LZS byte arrays
                                Console.Write("\rAllocating Normal Blocks: " + ((i_block * 4) + i_blockBotArrange + 1).ToString("000") + "        ");
                                lst_BOTBlocks.Add(lst_MAPBlocks[i_srcBlock]);
                            }
                        }

                        Console.WriteLine("\r");

                        Console.WriteLine("Allocating Special Blocks (253-332)...");

                        // special blocks
                        // we will do this manually
                        // BLOCK 63
                        lst_BOTBlocks.Add(lst_MAPBlocks[40]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[41]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[49]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[63]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[41]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[42]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[63]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[51]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[49]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[63]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[58]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[59]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[63]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[51]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[59]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[60]);


                        // BLOCKS 64-65 (we update also BLOCK 63 when Block 50 is present)
                        lst_BOTBlocks.Add(lst_MAPBlocks[31]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[32]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[40]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[64]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[32]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[33]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[64]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[65]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[33]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[34]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[65]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[43]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[40]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[64]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[49]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[63]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[64]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[65]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[63]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[51]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[65]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[43]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[51]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[52]);


                        // BLOCKS 66 (we update also BLOCK 63 when Block 50 is present)
                        lst_BOTBlocks.Add(lst_MAPBlocks[63]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[51]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[59]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[66]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[51]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[52]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[66]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[61]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[59]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[66]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[5]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[6]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[66]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[61]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[6]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[7]);


                        // BLOCKS 67-68
                        lst_BOTBlocks.Add(lst_MAPBlocks[37]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[38]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[46]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[67]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[38]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[39]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[67]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[68]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[39]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[40]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[68]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[49]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[46]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[67]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[55]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[56]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[67]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[68]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[56]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[57]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[68]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[49]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[57]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[58]);
                }
                else if (i_ncols == 3)
                {
                        Console.WriteLine("Allocating Blocks...");

                        lst_BOTBlocks.Add(lst_MAPBlocks[0]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[1]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[3]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[4]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[1]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[2]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[4]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[5]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[2]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[0]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[5]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[3]);
                        

                        lst_BOTBlocks.Add(lst_MAPBlocks[3]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[4]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[6]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[7]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[4]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[5]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[7]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[8]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[5]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[3]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[8]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[6]);


                        lst_BOTBlocks.Add(lst_MAPBlocks[6]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[7]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[9]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[10]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[7]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[8]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[10]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[11]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[8]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[6]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[11]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[9]);


                        lst_BOTBlocks.Add(lst_MAPBlocks[9]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[10]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[0]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[1]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[10]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[11]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[1]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[2]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[11]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[9]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[2]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[0]);
                    }
                else
                {
                        Console.WriteLine("Allocating Blocks...");

                        lst_BOTBlocks.Add(lst_MAPBlocks[0]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[1]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[2]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[3]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[1]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[0]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[3]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[2]);


                        lst_BOTBlocks.Add(lst_MAPBlocks[2]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[3]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[0]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[1]);

                        lst_BOTBlocks.Add(lst_MAPBlocks[3]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[2]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[1]);
                        lst_BOTBlocks.Add(lst_MAPBlocks[0]);
                    }


                    // now we can write the .BOT file with the new data
                    i_result = writeBlocks(lst_BOTBlocks);
                }
                catch
                {
                    i_result = -1;
                }
            }

            return i_result;
        }


        public int loadBlocks(List<byte[]> lst_Blocks)
        {
            int i_result;

            i_result = 0;

            Console.WriteLine("Loading Blocks of MAP file: " + str_botfilename + "...");

            try
            {
                using (BinaryReader br_MAP = new BinaryReader(File.Open(str_mapfilename, FileMode.Open)))
                {
                    while (br_MAP.BaseStream.Position != br_MAP.BaseStream.Length)
                        lst_Blocks.Add(br_MAP.ReadBytes(BLOCK_SIZE));
                }
            }
            catch
            {
                i_result = -1;
            }

            return i_result;
        }
        

        public int writeBlocks(List<byte[]> lst_Blocks)
        {
            int i_result;

            i_result = 0;

            // some text
            Console.WriteLine("Writing BOT file: " + str_botfilename + "...");

            try
            {
                using (BinaryWriter bw_BOT = new BinaryWriter(File.Open(str_filename + ".BOT", FileMode.Create)))
                {
                    foreach (byte[] byte_Block in lst_Blocks)
                    {
                        bw_BOT.Write(byte_Block);
                    }
                }
            }
            catch
            {
                i_result = -1;
            }

            return i_result;
        }


        //// There are 69 blocks in wm0.map, 12 blocks in wm2.map and 4 blocks in wm3.map
        //// The structure of the map is a grid of 7 rows x 9 columns (63 Blocks) (for example for WM0.MAP).
        //// Each Block consists a grid of 4 rows x 4 columns (16 Meshes).
        //// Each mesh has 8192 x 8192 coordinate units. So, each block has (8192x4 = 32768) 32768x32768 coordinate units.

        //public int export2Obj()
        //{
        //    // vars for create OBJ
        //    OBJ map2obj = new OBJ();
        //    ST_BLOCK stBlockList;
        //    ST_MESH stMeshList;

        //    Face tmpF;

        //    // function vars
        //    int i_result;
        //    double ut0, vt0, ut1, vt1, ut2, vt2;
        //    int i_vertex, i_triangle, i_totalVs, i_totalVTs;
        //    double i_texwidth, i_texheight, i_texuoffset, i_texvoffset;
        //    int i_block, i_mesh, row_Block, col_Block, row_Mesh, col_Mesh;

        //    i_result = 0;

        //    // initialize some things
        //    i_totalVs = 0;
        //    i_totalVTs = 0;


        //    // we will add materials if b_fullmaterial flag active
        //    // as we will put all the materials in one file, we don't need to do that for each block
        //    // init list of blocks' materials (list of blocks)
        //    if (b_fullmaterial)
        //    {


        //        // init list of Materials in the Block (list of strings) if not by blocks
        //        if (!b_byblocks)
        //        {
        //            lst_MatBlock = new HashSet<string>[1];
        //            lst_MatBlock[0] = new HashSet<string>();
        //        }
        //        else lst_MatBlock = new HashSet<string>[i_nblocks];
        //    }


        //    try
        //    {
        //        // some text output
        //        Console.WriteLine("Exporting FULL worldmap .BOT file into .OBJ files...");

        //        // first we put the material library filename
        //        map2obj.str_objmtlfile = str_filename + ".mtl";

        //        // create the lists of the Blocks for OBJ class
        //        map2obj.stOBJ = new List<ST_BLOCK>();

        //        // let's put the .map data into the .obj structure with Block/Mesh own data
        //        for (i_block = 0; i_block < i_nblocks; i_block++)
        //        {

        //            // let's get the col/row block we are treating
        //            col_Block = i_block % i_ncols;
        //            row_Block = i_block / i_ncols;

        //            // create a block and list of meshes for this block
        //            stBlockList = new ST_BLOCK();
        //            stBlockList.stMesh = new List<ST_MESH>();

        //            // prepare list of materials by block
        //            // init V,VTs count
        //            // total vertices counter
        //            i_totalVs = 0;
        //            i_totalVTs = 0;

        //            // new list of materials for this block
        //            lst_MatBlock[i_block] = new HashSet<string>();


        //            for (i_mesh = 0; i_mesh < N_MESHES; i_mesh++)
        //            {
        //                // let's get the col/row mesh we are treating
        //                col_Mesh = i_mesh % 4;
        //                row_Mesh = i_mesh / 4;

        //                // create a list of meshes
        //                stMeshList = new ST_MESH();
        //                stMeshList.VList = new List<Vertex>();
        //                stMeshList.VNList = new List<VertexNormal>();
        //                stMeshList.VTList = new List<VertexTexture>();
        //                stMeshList.FList = new List<Face>();

        //                // put the name of the group like "Block00_Mesh00"
        //                stMeshList.str_groupName = "Block" + i_block.ToString("00") + "_Mesh" + i_mesh.ToString("00");

        //                // write vertices
        //                for (i_vertex = 0; i_vertex < wm_block[i_block].wm_mesh[i_mesh].i_numVertices; i_vertex++)
        //                {
        //                    stMeshList.VList.Add(new Vertex()
        //                    {
        //                        Index = i_vertex,
        //                        X = wm_block[i_block].wm_mesh[i_mesh].wm_vertices[i_vertex].x + (col_Block * BLOCK_PIXELS) + (col_Mesh * MESH_PIXELS),
        //                        Y = wm_block[i_block].wm_mesh[i_mesh].wm_vertices[i_vertex].y,
        //                        Z = wm_block[i_block].wm_mesh[i_mesh].wm_vertices[i_vertex].z + (row_Block * BLOCK_PIXELS) + (row_Mesh * MESH_PIXELS)
        //                    });

        //                }

        //                // write vertices normals
        //                for (i_vertex = 0; i_vertex < wm_block[i_block].wm_mesh[i_mesh].i_numVertices; i_vertex++)
        //                {
        //                    stMeshList.VNList.Add(new VertexNormal()
        //                    {
        //                        Index = i_vertex,
        //                        X = wm_block[i_block].wm_mesh[i_mesh].wm_vertices[i_vertex].xn / VERTEX_NORMAL_DEF,
        //                        Y = wm_block[i_block].wm_mesh[i_mesh].wm_vertices[i_vertex].yn / VERTEX_NORMAL_DEF,
        //                        Z = wm_block[i_block].wm_mesh[i_mesh].wm_vertices[i_vertex].zn / VERTEX_NORMAL_DEF
        //                    });
        //                }


        //                // write uvmap
        //                for (i_triangle = 0; i_triangle < wm_block[i_block].wm_mesh[i_mesh].i_numTriangles; i_triangle++)
        //                {

        //                    // get texture width/height/uvoffsets
        //                    i_texwidth = stMaterialID[wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].textureID].MWidth;
        //                    i_texheight = stMaterialID[wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].textureID].MHeight;
        //                    i_texuoffset = stMaterialID[wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].textureID].UOffset;
        //                    i_texvoffset = stMaterialID[wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].textureID].VOffset;


        //                    // prepare utexture vertices
        //                    ut0 = (wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].ut0 - i_texuoffset) / i_texwidth;
        //                    ut1 = (wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].ut1 - i_texuoffset) / i_texwidth;
        //                    ut2 = (wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].ut2 - i_texuoffset) / i_texwidth;

        //                    // prepare vtexture vertices
        //                    vt0 = (wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].vt0 - i_texvoffset) / i_texheight;
        //                    if (vt0 < 0) vt0 = (vt0 * -1) + 1;
        //                    else vt0 = 1 - vt0;

        //                    vt1 = (wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].vt1 - i_texvoffset) / i_texheight;
        //                    if (vt1 < 0) vt1 = (vt1 * -1) + 1;
        //                    else vt1 = 1 - vt1;

        //                    vt2 = (wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].vt2 - i_texvoffset) / i_texheight;
        //                    if (vt2 < 0) vt2 = (vt2 * -1) + 1;
        //                    else vt2 = 1 - vt2;

        //                    // put texture vertices
        //                    stMeshList.VTList.Add(new VertexTexture()
        //                    {
        //                        Index = i_totalVTs + 1,
        //                        X = ut0,
        //                        Y = vt0
        //                    });

        //                    stMeshList.VTList.Add(new VertexTexture()
        //                    {
        //                        Index = i_totalVTs + 2,
        //                        X = ut1,
        //                        Y = vt1
        //                    });

        //                    stMeshList.VTList.Add(new VertexTexture()
        //                    {
        //                        Index = i_totalVTs + 3,
        //                        X = ut2,
        //                        Y = vt2
        //                    });


        //                    // put faces
        //                    tmpF = new Face();
        //                    tmpF.VertexIndexList = new int[3];
        //                    tmpF.VertexNormalIndexList = new int[3];
        //                    tmpF.VertexTextureIndexList = new int[3];

        //                    // let's put the material first
        //                    if (b_fullmaterial)
        //                    {
        //                        tmpF.UseMtl = stMaterialID[wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].textureID].MName + "_" +
        //                                      wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].textureID + "_" +
        //                                      wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].locationID + "_" +
        //                                      stWalkmapID[wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].walkmapID].WName + "_" +
        //                                      wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].walkmapID + "_" +
        //                                      wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].walkmapUNK;


        //                        // let's put the special triangle information as material identifier
        //                        // we put ":" after MName to help create later the material list
        //                        string str_mat = stMaterialID[wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].textureID].MName + ":" +
        //                                         wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].textureID + "_" +
        //                                         wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].locationID + "_" +
        //                                         stWalkmapID[wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].walkmapID].WName + "_" +
        //                                         wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].walkmapID + "_" +
        //                                         wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].walkmapUNK;

        //                        if (b_byblocks)
        //                            lst_MatBlock[i_block].Add(str_mat);
        //                        else
        //                            lst_MatBlock[0].Add(str_mat);

        //                    }
        //                    else
        //                    {
        //                        tmpF.UseMtl = stMaterialID[wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].textureID].MName;
        //                    }

        //                    tmpF.VertexIndexList[0] = wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].v0 + i_totalVs + 1;
        //                    tmpF.VertexTextureIndexList[0] = i_totalVTs + 1;
        //                    tmpF.VertexNormalIndexList[0] = wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].v0 + i_totalVs + 1;

        //                    tmpF.VertexIndexList[1] = wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].v1 + i_totalVs + 1;
        //                    tmpF.VertexTextureIndexList[1] = i_totalVTs + 2;
        //                    tmpF.VertexNormalIndexList[1] = wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].v1 + i_totalVs + 1;

        //                    tmpF.VertexIndexList[2] = wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].v2 + i_totalVs + 1;
        //                    tmpF.VertexTextureIndexList[2] = i_totalVTs + 3;
        //                    tmpF.VertexNormalIndexList[2] = wm_block[i_block].wm_mesh[i_mesh].wm_triangles[i_triangle].v2 + i_totalVs + 1;

        //                    stMeshList.FList.Add(tmpF);

        //                    // texture vertices counter
        //                    i_totalVTs += 3;
        //                }

        //                //if (map2obj.stBlock == null) map2obj.stBlock = new
        //                stBlockList.stMesh.Add(stMeshList);

        //                i_totalVs += wm_block[i_block].wm_mesh[i_mesh].i_numVertices;
        //            }

        //            map2obj.stOBJ.Add(stBlockList);
        //        }

        //        // finally we can write the obj file
        //        if (b_byblocks) i_result = map2obj.WriteByBlocksObjFile();
        //        else i_result = map2obj.WriteFullObjFile();
        //    }
        //    catch
        //    {
        //        i_result = -1;
        //    }

        //    return i_result;
        //}

    }
}
