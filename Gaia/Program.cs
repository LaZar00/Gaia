using System;
using System.IO;
using static Gaia.Globals;
using static Gaia.Materials;
using static Gaia.Walkmap;


namespace Gaia
{
    class Program
    {

        private static void Help()
        {
            Console.WriteLine("\r\n");
            Console.WriteLine("Gaia Worldmap Converter Tool for FF7 v1.0");
            Console.WriteLine("=========================================");
            Console.WriteLine("Usage: gaia <file.[map|obj]> [options]");
            Console.WriteLine("");
            Console.WriteLine("<file.[MAP|OBJ|BOT]>     File to work with. Mandatory (not case sensitive).");
            Console.WriteLine("                         .MAP => export to .OBJ      .OBJ => convert to .MAP. ");
            Console.WriteLine("                         .MAP => rebuild .BOT");
            Console.WriteLine("");
            Console.WriteLine("Options ONLY for .MAP processing:");
            Console.WriteLine("-t:[texture_path]    Path where the textures are. The folder MUST exist. (default: 'textures' folder)");
            Console.WriteLine("-f:[image_format]    Image format (extension) of the texture images (JPG, PNG, TGA, BMP... NO admits TEX).");
            Console.WriteLine("                     This is only for creating material file .MTL for the .OBJ. (default: jpg)");
            Console.WriteLine("-b:[blocks_path]     Path where we will export the .MAP file in Blocks.");
            Console.WriteLine("                     When this option is used, the output is one .OBJ file for each Block of the .MAP.");
            Console.WriteLine("                     If this option is not used, the output is the Full map in one .OBJ file.");
            Console.WriteLine("                     This options enables the option '-m' implicitly. The folder MUST exist.");
            Console.WriteLine("-n                   This option disables full material name (without internal triangles data info).");
            Console.WriteLine("                     When used, the OBJ to MAP conversion will NOT work.");
            Console.WriteLine("-r                   This option rebuilds the .BOT file from the .MAP file.");
            Console.WriteLine("                     The output uses the same filename as .MAP file but with .BOT extension.");
            Console.WriteLine("                     Other options used will have not effect.");
            Console.WriteLine("");
            Console.WriteLine("Options for .MAP/.OBJ/.BOT processing:");
            Console.WriteLine("-d:[dumps_path]      This option dumps the binary data of each Block/Mesh pair uncompressed to");
            Console.WriteLine("                     a given path. The folder MUST exist. (default: 'dumpsOBJ' folder).");
            Console.WriteLine("                     When dumping the data from .OBJ, it DOES NOT creates the .MAP file.");
            Console.WriteLine("");
            Console.WriteLine("Samples using Gaia tool:");
            Console.WriteLine("");
            Console.WriteLine("   gaia wm0.map");
            Console.WriteLine("   gaia wm0.map -t:..\\pngtextures -f:png -b:blocks");
            Console.WriteLine("   gaia wm0.map -t:..\\pngtextures -f:png -b:blocks");
            Console.WriteLine("   gaia wm0.map -t:texturesHD -f:png");
            Console.WriteLine("   gaia wm0.map -d");
            Console.WriteLine("   gaia wm0.map -d:dumpsWM0");
            Console.WriteLine("   gaia wm0.obj");
            Console.WriteLine("   gaia wm0.obj -d:dumpsWM0_OBJ");
            Console.WriteLine("   gaia wm0.bot -d:dumpsWM0_BOT");
            Console.WriteLine("   gaia wm0.map -r");
            Console.WriteLine("");
            //Console.WriteLine("Press any key to continue...");
            //Console.ReadKey(true);
        }

        static void Main(string[] args)
        {

            Map2Obj wm_map;
            Map2Bot wm_bot;
            Obj2Map wm_obj;

            int i_cntargs, i_result;
            string[] str_arg;


            if (args.Length > 0)
            {

                // init some vars
                str_texturepath = "textures";
                str_imgformat = "jpg";
                str_blockspath = "";
                str_dumpspath = "dumps";

                b_fullmaterial = true;
                b_byblocks = false;
                b_dumpdata = false;
                b_isbot = false;
                b_rebuild = false;

                i_result = 0;

                // get filename (without extension)
                str_filename = Path.GetFileNameWithoutExtension(args[0]);
                str_fileext = Path.GetExtension(args[0]);


                /////////////////////////////////////////////////////////////////////////////////////////
                ///////////////// MAP2OBJ
                /////////////////////////////////////////////////////////////////////////////////////////
                if (str_fileext.ToUpper() == ".MAP")
                {
                    // This means we want to convert .map to .obj
                    str_mapfilename = args[0];

                    // check if file exists
                    if (!File.Exists(str_mapfilename))
                    {
                        Console.WriteLine("Error: File '" + str_mapfilename + "' does not exists.\r\n");
                        return;
                    }

                    // check options
                    if (args.Length > 1)
                    {
                        i_cntargs = 1;

                        while (i_cntargs < args.Length)
                        {
                            str_arg = args[i_cntargs].Split(':');

                            switch (str_arg[0])
                            {
                                case "-t":
                                    if (str_arg.Length > 1 && !string.IsNullOrEmpty(str_arg[1])) str_texturepath = str_arg[1];
                                    break;

                                case "-f":
                                    if (str_arg.Length > 1 && !string.IsNullOrEmpty(str_arg[1])) str_imgformat = str_arg[1];
                                    break;

                                case "-b":
                                    if (str_arg.Length > 1 && !string.IsNullOrEmpty(str_arg[1])) str_blockspath = str_arg[1];

                                    if (!Directory.Exists(str_blockspath))
                                    {
                                        Console.WriteLine("Warning: Selected directory '" + str_blockspath + "' for export Blocks/Meshes in .OBJ format does not exists.");
                                        return;
                                    }                                        

                                    b_byblocks = true;
                                    b_fullmaterial = true;
                                    break;

                                case "-n":
                                    b_fullmaterial = false;
                                    break;

                                case "-d":
                                    if (str_arg.Length > 1 && !string.IsNullOrEmpty(str_arg[1])) str_dumpspath = str_arg[1];

                                    if (!Directory.Exists(str_dumpspath))
                                    {
                                        Console.WriteLine("Warning: Selected directory '" + str_dumpspath + "' for dump Blocks/Meshes binary files does not exists.");
                                        return;
                                    }
                                    b_dumpdata = true;
                                    break;

                                case "-r":                                        
                                    b_rebuild = true;
                                    break;

                                default:
                                    Help();
                                    return;
                            }

                            i_cntargs += 1;
                        }
                    }

                    // Some previous warnings
                    if (!Directory.Exists(str_texturepath))
                    {
                        Console.WriteLine("Warning: Selected directory '" + str_texturepath + "', where the textures should be, MAYBE does not exists.");
                        Console.WriteLine("         The .OBJ file will be created, but the 3D Application maybe is not able to find the textures.");
                    }


                    // option rebuild
                    if (b_rebuild)
                    {

                        wm_bot = new Map2Bot();

                        i_result = wm_bot.rebuildBOT();

                        if (i_result == 0)
                            Console.WriteLine("Rebuild process of .BOT file from .MAP file... FINISHED.");
                        else
                            Console.WriteLine("Exception Error when rebuilding the .BOT file...");

                        return;
                    }


                    // load .map file
                    wm_map = new Map2Obj();


                    // Populate Tables
                    Console.WriteLine("Populating Materials...");
                    PopulateMaterialIDs();

                    Console.WriteLine("Populating Walkmap...");
                    PopulateWalkmapID();


                    // export .MAP data to .OBJ
                    if (b_dumpdata) i_result = dumpBinaryData(i_nblocks);
                    else i_result = wm_map.export2Obj();

                    if (i_result == 0)
                    {
                        if (b_dumpdata)
                        {
                            Console.WriteLine("Dump of Binary Data of Blocks/Meshes... FINISHED.");
                        }
                        else
                        {
                            if (b_byblocks)
                            {
                                Console.WriteLine("Generating .mtl files...");
                                i_result = writeMatLibBlocks(str_filename, str_texturepath, str_imgformat, str_blockspath);

                                if (i_result == 0)
                                    Console.WriteLine("Export .MAP to .OBJ by BLOCKS... FINISHED.");
                                else
                                    Console.WriteLine("Exception Error when generating .MTL files...");
                            }
                            else
                            {
                                Console.WriteLine("Generating .mtl file...");
                                i_result = writeMatLib(str_filename + ".mtl", str_texturepath, str_imgformat, str_blockspath);

                                if (i_result == 0)
                                    Console.WriteLine("Export FULL .MAP to .OBJ... FINISHED.");
                                else
                                    Console.WriteLine("Exception Error when generating .MTL file...");

                            }
                        }
                    }
                    else
                    {
                        if (i_result == -1)
                            Console.WriteLine("Exception Error when exporting to .OBJ...");
                    }

                    return;
                }


                /////////////////////////////////////////////////////////////////////////////////////////
                ///////////////// OBJ2MAP
                /////////////////////////////////////////////////////////////////////////////////////////
                if (str_fileext.ToUpper() == ".OBJ")
                {
                    // This means we want to convert .obj to .map
                    str_objfilename = args[0];

                    // check if file exists
                    if (!File.Exists(str_objfilename))
                    {
                        Console.WriteLine("Error: File '" + str_objfilename + "' does not exists.\r\n");
                        return;
                    }

                    // check options
                    if (args.Length > 1)
                    {
                        i_cntargs = 1;

                        while (i_cntargs < args.Length)
                        {
                            str_arg = args[i_cntargs].Split(':');

                            switch (str_arg[0])
                            {
                                case "-d":
                                    if (str_arg.Length > 1 && !string.IsNullOrEmpty(str_arg[1])) str_dumpspath = str_arg[1];

                                    if (!Directory.Exists(str_dumpspath))
                                    {
                                        Console.WriteLine("Warning: Selected directory '" + str_dumpspath + "' for dump Blocks/Meshes binary files does not exists.");
                                        return;
                                    }

                                    b_dumpdata = true;
                                    break;

                                default:
                                    Help();
                                    return;
                            }

                            i_cntargs += 1;
                        }
                    }


                    // we will load the .obj file in the .map memory structs to make easy the saving
                    wm_obj = new Obj2Map();

                    // let's get the i_nblocks/i_ncols values
                    i_nblocks = wm_obj.i_objnblocks;
                    get_ncols();

                    // Populate Tables
                    Console.WriteLine("Populating Materials...");
                    PopulateMaterialIDs();

                    Console.WriteLine("Populating Walkmap...");
                    PopulateWalkmapID();


                    // now we can export to .MAP the data
                    i_result = wm_obj.export2Map();

                    if (i_result == 0)
                        Console.WriteLine("Export FULL .OBJ to .MAP... FINISHED.");
                    else if (i_result == -1)
                        Console.WriteLine("Exception Error when generating .MAP file...");

                    return;
                }


                /////////////////////////////////////////////////////////////////////////////////////////
                ///////////////// BOT
                /////////////////////////////////////////////////////////////////////////////////////////
                if (str_fileext.ToUpper() == ".BOT")
                {
                    // This means we want to convert .obj to .map
                    str_botfilename = args[0];

                    // check if file exists
                    if (!File.Exists(str_botfilename))
                    {
                        Console.WriteLine("Error: File '" + str_botfilename + "' does not exists.\r\n");
                        return;
                    }

                    // check options
                    if (args.Length > 1)
                    {
                        i_cntargs = 1;

                        while (i_cntargs < args.Length)
                        {
                            str_arg = args[i_cntargs].Split(':');

                            switch (str_arg[0])
                            {
                                case "-d":
                                    if (str_arg.Length > 1 && !string.IsNullOrEmpty(str_arg[1])) str_dumpspath = str_arg[1];

                                    if (!Directory.Exists(str_dumpspath))
                                    {
                                        Console.WriteLine("Warning: Selected directory '" + str_dumpspath + "' for dump Blocks/Meshes binary files does not exists.");
                                        return;
                                    }

                                    b_dumpdata = true;
                                    break;

                                default:
                                    Help();
                                    return;
                            }

                            i_cntargs += 1;
                        }
                    }


                    // let's tell the tool we will work with a BOT file
                    b_isbot = true;

                    // we will load the .BOT file in the .MAP memory structs to make easy the saving
                    wm_bot = new Map2Bot();

                    // export .BOT data to .OBJ or dump
                    if (b_dumpdata) i_result = dumpBinaryData(i_nblocksbot);
                    //else i_result = wm_bot.export2Obj();

                    if (i_result == 0)
                        Console.WriteLine("Dump of Blocks/Meshes for .BOT file... FINISHED.");
                    else
                        Console.WriteLine("Exception Error when dumping Blocks/Meshes of the .BOT file...");

                    return;
                }

                Help();
            }
            else
            {
                // print info
                Help();
            }        
        }
    }
}
