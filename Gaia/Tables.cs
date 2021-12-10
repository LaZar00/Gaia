// Table posted in ff7-mods:
// https://github.com/ff7-mods/ff7-flat-wiki/blob/master/docs/FF7/WorldMap_Module/TextureTable.md


using System.IO;
using System.Collections.Generic;
using static Gaia.Globals;

namespace Gaia
{

    //public static class BlockReplacements
    //{
    //    public static SortedList<int, int> sl_Replacements;

    //    public static void PopulateBlockReplacements()
    //    {
    //        sl_Replacements = new SortedList<int, int>();

    //        sl_Replacements.Add(63, 50);
    //        sl_Replacements.Add(64, 41);
    //        sl_Replacements.Add(65, 42);
    //        sl_Replacements.Add(66, 60);
    //        sl_Replacements.Add(67, 47);
    //        sl_Replacements.Add(68, 48);
    //    }
    //}


    public static class Materials
    {

        public struct MaterialID
        {
            public string MName;
            public int MWidth;
            public int MHeight;
            public int UOffset;
            public int VOffset;
        }

        public static List<MaterialID> stMaterialID = new List<MaterialID>();

        public static void PopulateMaterialIDs()
        {
            if (i_ncols > 3)
            {
                stMaterialID.Add(new MaterialID() { MName = "pond", MWidth = 32, MHeight = 32, UOffset = 128, VOffset = 352 });
                stMaterialID.Add(new MaterialID() { MName = "riv_m2", MWidth = 32, MHeight = 32, UOffset = 128, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "was_gs", MWidth = 64, MHeight = 64, UOffset = 64, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "wpcltr", MWidth = 32, MHeight = 128, UOffset = 0, VOffset = 256 });
                stMaterialID.Add(new MaterialID() { MName = "wpcltr2", MWidth = 32, MHeight = 32, UOffset = 160, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "bzdun", MWidth = 64, MHeight = 64, UOffset = 192, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "bone", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 384 });
                stMaterialID.Add(new MaterialID() { MName = "bone2", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 416 });
                stMaterialID.Add(new MaterialID() { MName = "bornwd", MWidth = 64, MHeight = 64, UOffset = 160, VOffset = 320 });
                stMaterialID.Add(new MaterialID() { MName = "bridge", MWidth = 32, MHeight = 64, UOffset = 192, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "bridge2", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "cave", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 448 });
                stMaterialID.Add(new MaterialID() { MName = "cave2", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 320 });
                stMaterialID.Add(new MaterialID() { MName = "cave_s", MWidth = 32, MHeight = 32, UOffset = 160, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "cdl_cl2", MWidth = 64, MHeight = 32, UOffset = 96, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "cf01", MWidth = 64, MHeight = 32, UOffset = 192, VOffset = 288 });
                stMaterialID.Add(new MaterialID() { MName = "clf_bgs", MWidth = 64, MHeight = 32, UOffset = 192, VOffset = 384 });
                stMaterialID.Add(new MaterialID() { MName = "clf_ggl", MWidth = 64, MHeight = 64, UOffset = 128, VOffset = 256 });
                stMaterialID.Add(new MaterialID() { MName = "clf_ggs", MWidth = 64, MHeight = 32, UOffset = 192, VOffset = 352 });
                stMaterialID.Add(new MaterialID() { MName = "clf_l", MWidth = 64, MHeight = 64, UOffset = 0, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "clf_ld", MWidth = 64, MHeight = 64, UOffset = 64, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "clf_lf", MWidth = 64, MHeight = 64, UOffset = 128, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "clf_lg", MWidth = 32, MHeight = 64, UOffset = 0, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "clf_lr", MWidth = 32, MHeight = 64, UOffset = 128, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "clf_lsg", MWidth = 32, MHeight = 64, UOffset = 64, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "clf_r", MWidth = 32, MHeight = 32, UOffset = 0, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "clf_s", MWidth = 64, MHeight = 32, UOffset = 192, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "clf_sd", MWidth = 64, MHeight = 32, UOffset = 192, VOffset = 32 });
                stMaterialID.Add(new MaterialID() { MName = "clf_sf", MWidth = 64, MHeight = 32, UOffset = 0, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "clf_sg", MWidth = 32, MHeight = 32, UOffset = 32, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "clf_sg2", MWidth = 32, MHeight = 32, UOffset = 0, VOffset = 160 });
                stMaterialID.Add(new MaterialID() { MName = "clf_sr", MWidth = 32, MHeight = 32, UOffset = 32, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "clf_ss", MWidth = 32, MHeight = 32, UOffset = 32, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "clf_ssd", MWidth = 32, MHeight = 32, UOffset = 0, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "clf_ssw", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 32 });
                stMaterialID.Add(new MaterialID() { MName = "clf_sw", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 32 });
                stMaterialID.Add(new MaterialID() { MName = "clf_w02", MWidth = 64, MHeight = 64, UOffset = 128, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "clf_w03", MWidth = 64, MHeight = 64, UOffset = 192, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "clf_was", MWidth = 64, MHeight = 32, UOffset = 64, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "clfeg", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 320 });
                stMaterialID.Add(new MaterialID() { MName = "clfegd", MWidth = 32, MHeight = 32, UOffset = 0, VOffset = 320 });
                stMaterialID.Add(new MaterialID() { MName = "clftop", MWidth = 64, MHeight = 32, UOffset = 192, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "clftop2", MWidth = 32, MHeight = 32, UOffset = 128, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "cndl_cl", MWidth = 64, MHeight = 32, UOffset = 96, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "cndlf", MWidth = 64, MHeight = 64, UOffset = 160, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "cndlf02", MWidth = 64, MHeight = 64, UOffset = 208, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "comtr", MWidth = 16, MHeight = 32, UOffset = 144, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "cosinn", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 416 });
                stMaterialID.Add(new MaterialID() { MName = "cosinn2", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 448 });
                stMaterialID.Add(new MaterialID() { MName = "csmk", MWidth = 32, MHeight = 32, UOffset = 64, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "csmk2", MWidth = 32, MHeight = 32, UOffset = 96, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "cstds01", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 160 });
                stMaterialID.Add(new MaterialID() { MName = "cstds02", MWidth = 64, MHeight = 64, UOffset = 0, VOffset = 448 });
                stMaterialID.Add(new MaterialID() { MName = "des01", MWidth = 32, MHeight = 32, UOffset = 160, VOffset = 320 });
                stMaterialID.Add(new MaterialID() { MName = "desert", MWidth = 64, MHeight = 64, UOffset = 128, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "desor", MWidth = 64, MHeight = 32, UOffset = 160, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "ds1", MWidth = 32, MHeight = 32, UOffset = 0, VOffset = 256 });
                stMaterialID.Add(new MaterialID() { MName = "ds_s1", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 288 });
                stMaterialID.Add(new MaterialID() { MName = "dsee1", MWidth = 32, MHeight = 32, UOffset = 96, VOffset = 288 });
                stMaterialID.Add(new MaterialID() { MName = "dsrt_d", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "dsrt_e", MWidth = 64, MHeight = 128, UOffset = 64, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "edes01", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 320 });
                stMaterialID.Add(new MaterialID() { MName = "elm01", MWidth = 32, MHeight = 32, UOffset = 160, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "elm02", MWidth = 32, MHeight = 32, UOffset = 64, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "elm_gro", MWidth = 64, MHeight = 64, UOffset = 0, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "elm_r", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "elm_r2", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "fall1", MWidth = 32, MHeight = 32, UOffset = 128, VOffset = 256 });
                stMaterialID.Add(new MaterialID() { MName = "farm01", MWidth = 32, MHeight = 32, UOffset = 160, VOffset = 32 });
                stMaterialID.Add(new MaterialID() { MName = "farm02", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 32 });
                stMaterialID.Add(new MaterialID() { MName = "farm_g", MWidth = 32, MHeight = 32, UOffset = 128, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "farm_r", MWidth = 32, MHeight = 16, UOffset = 128, VOffset = 48 });
                stMaterialID.Add(new MaterialID() { MName = "fld", MWidth = 64, MHeight = 64, UOffset = 64, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "fld_02", MWidth = 64, MHeight = 64, UOffset = 0, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "fld_s", MWidth = 64, MHeight = 64, UOffset = 0, VOffset = 160 });
                stMaterialID.Add(new MaterialID() { MName = "fld_s2", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 256 });
                stMaterialID.Add(new MaterialID() { MName = "fld_sw", MWidth = 64, MHeight = 64, UOffset = 128, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "fld_v", MWidth = 128, MHeight = 128, UOffset = 0, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "fld_vd", MWidth = 32, MHeight = 64, UOffset = 96, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "fld_vd2", MWidth = 32, MHeight = 64, UOffset = 192, VOffset = 416 });
                stMaterialID.Add(new MaterialID() { MName = "fvedge", MWidth = 32, MHeight = 64, UOffset = 0, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "gclf_d", MWidth = 128, MHeight = 64, UOffset = 128, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "gclf_g", MWidth = 32, MHeight = 64, UOffset = 224, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "gclfwa", MWidth = 128, MHeight = 64, UOffset = 64, VOffset = 320 });
                stMaterialID.Add(new MaterialID() { MName = "gclfwa2", MWidth = 32, MHeight = 64, UOffset = 160, VOffset = 320 });
                stMaterialID.Add(new MaterialID() { MName = "gclfwag", MWidth = 32, MHeight = 64, UOffset = 32, VOffset = 320 });
                stMaterialID.Add(new MaterialID() { MName = "gg_gro", MWidth = 64, MHeight = 64, UOffset = 64, VOffset = 448 });
                stMaterialID.Add(new MaterialID() { MName = "gg_mts", MWidth = 64, MHeight = 128, UOffset = 0, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "ggmk", MWidth = 64, MHeight = 64, UOffset = 128, VOffset = 448 });
                stMaterialID.Add(new MaterialID() { MName = "ggmt", MWidth = 128, MHeight = 128, UOffset = 0, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "ggmt_e", MWidth = 128, MHeight = 32, UOffset = 0, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "ggmt_ed", MWidth = 128, MHeight = 32, UOffset = 128, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "ggmt_eg", MWidth = 32, MHeight = 32, UOffset = 96, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "ggmtd", MWidth = 128, MHeight = 128, UOffset = 128, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "ggs_g", MWidth = 32, MHeight = 32, UOffset = 32, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "ggshr", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "ggshrg", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "gia", MWidth = 64, MHeight = 32, UOffset = 64, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "gia2", MWidth = 64, MHeight = 32, UOffset = 0, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "gia_d", MWidth = 64, MHeight = 32, UOffset = 128, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "gia_d2", MWidth = 64, MHeight = 32, UOffset = 64, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "gia_g", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "gia_g2", MWidth = 32, MHeight = 32, UOffset = 128, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "gmt_eda", MWidth = 32, MHeight = 32, UOffset = 0, VOffset = 352 });
                stMaterialID.Add(new MaterialID() { MName = "gonclf", MWidth = 128, MHeight = 64, UOffset = 96, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "gredge", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "hyouga", MWidth = 64, MHeight = 64, UOffset = 192, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "iceclf", MWidth = 64, MHeight = 32, UOffset = 64, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "iceclfd", MWidth = 64, MHeight = 32, UOffset = 128, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "iceclfg", MWidth = 32, MHeight = 32, UOffset = 32, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "jun", MWidth = 64, MHeight = 64, UOffset = 192, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "jun_d", MWidth = 64, MHeight = 64, UOffset = 128, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "jun_e", MWidth = 64, MHeight = 16, UOffset = 0, VOffset = 240 });
                stMaterialID.Add(new MaterialID() { MName = "jun_gro", MWidth = 64, MHeight = 64, UOffset = 0, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "junmk", MWidth = 32, MHeight = 32, UOffset = 0, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "junn01", MWidth = 32, MHeight = 32, UOffset = 160, VOffset = 112 });
                stMaterialID.Add(new MaterialID() { MName = "junn02", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 112 });
                stMaterialID.Add(new MaterialID() { MName = "junn03", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 112 });
                stMaterialID.Add(new MaterialID() { MName = "junn04", MWidth = 32, MHeight = 32, UOffset = 64, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "jutmpl01", MWidth = 64, MHeight = 64, UOffset = 128, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "lake-e", MWidth = 32, MHeight = 32, UOffset = 96, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "lake_ef", MWidth = 32, MHeight = 32, UOffset = 128, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "lake_fl", MWidth = 128, MHeight = 32, UOffset = 160, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "lostclf", MWidth = 32, MHeight = 64, UOffset = 128, VOffset = 384 });
                stMaterialID.Add(new MaterialID() { MName = "lostmt", MWidth = 128, MHeight = 32, UOffset = 128, VOffset = 448 });
                stMaterialID.Add(new MaterialID() { MName = "lostmtd", MWidth = 128, MHeight = 32, UOffset = 128, VOffset = 480 });
                stMaterialID.Add(new MaterialID() { MName = "lostmts", MWidth = 64, MHeight = 32, UOffset = 160, VOffset = 384 });
                stMaterialID.Add(new MaterialID() { MName = "lostwd_e", MWidth = 32, MHeight = 32, UOffset = 64, VOffset = 480 });
                stMaterialID.Add(new MaterialID() { MName = "lostwod", MWidth = 64, MHeight = 64, UOffset = 0, VOffset = 448 });
                stMaterialID.Add(new MaterialID() { MName = "lst1", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 256 });
                stMaterialID.Add(new MaterialID() { MName = "lstwd_e2", MWidth = 32, MHeight = 32, UOffset = 96, VOffset = 480 });
                stMaterialID.Add(new MaterialID() { MName = "mzes", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "mzmt_e", MWidth = 128, MHeight = 64, UOffset = 128, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "mzmt_ed", MWidth = 128, MHeight = 32, UOffset = 128, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "mzmt_edw", MWidth = 128, MHeight = 32, UOffset = 128, VOffset = 160 });
                stMaterialID.Add(new MaterialID() { MName = "mzmt_ew", MWidth = 128, MHeight = 32, UOffset = 0, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "mzmt_o", MWidth = 128, MHeight = 32, UOffset = 64, VOffset = 416 });
                stMaterialID.Add(new MaterialID() { MName = "mzmt_od", MWidth = 128, MHeight = 32, UOffset = 64, VOffset = 448 });
                stMaterialID.Add(new MaterialID() { MName = "mzmt_s", MWidth = 128, MHeight = 32, UOffset = 0, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "mzmt_sd", MWidth = 128, MHeight = 32, UOffset = 0, VOffset = 160 });
                stMaterialID.Add(new MaterialID() { MName = "md01", MWidth = 32, MHeight = 32, UOffset = 96, VOffset = 16 });
                stMaterialID.Add(new MaterialID() { MName = "md02", MWidth = 128, MHeight = 128, UOffset = 0, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "md03", MWidth = 16, MHeight = 16, UOffset = 112, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "md04", MWidth = 32, MHeight = 32, UOffset = 128, VOffset = 16 });
                stMaterialID.Add(new MaterialID() { MName = "md05", MWidth = 64, MHeight = 16, UOffset = 96, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "md06", MWidth = 16, MHeight = 32, UOffset = 96, VOffset = 48 });
                stMaterialID.Add(new MaterialID() { MName = "md07", MWidth = 16, MHeight = 16, UOffset = 112, VOffset = 48 });
                stMaterialID.Add(new MaterialID() { MName = "md_mt", MWidth = 128, MHeight = 128, UOffset = 128, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "md_mtd", MWidth = 128, MHeight = 128, UOffset = 0, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "md_mts", MWidth = 64, MHeight = 128, UOffset = 64, VOffset = 160 });
                stMaterialID.Add(new MaterialID() { MName = "md_snow", MWidth = 128, MHeight = 32, UOffset = 128, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "md_snw2", MWidth = 128, MHeight = 32, UOffset = 128, VOffset = 32 });
                stMaterialID.Add(new MaterialID() { MName = "md_snwd", MWidth = 128, MHeight = 64, UOffset = 0, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "md_snwe", MWidth = 64, MHeight = 64, UOffset = 96, VOffset = 320 });
                stMaterialID.Add(new MaterialID() { MName = "md_snws", MWidth = 64, MHeight = 64, UOffset = 128, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "md_snwt", MWidth = 128, MHeight = 32, UOffset = 0, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "md_snww", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "md_sw_s", MWidth = 128, MHeight = 128, UOffset = 0, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "md_swd2", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 256 });
                stMaterialID.Add(new MaterialID() { MName = "md_swnp", MWidth = 128, MHeight = 128, UOffset = 0, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "mdsrt_e", MWidth = 128, MHeight = 32, UOffset = 128, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "mdsrt_ed", MWidth = 128, MHeight = 32, UOffset = 128, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "mdsrt_eg", MWidth = 32, MHeight = 32, UOffset = 64, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "midil", MWidth = 32, MHeight = 32, UOffset = 32, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "midild", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "mt_ewg", MWidth = 32, MHeight = 32, UOffset = 64, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "mt_road", MWidth = 64, MHeight = 64, UOffset = 192, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "mt_se", MWidth = 32, MHeight = 32, UOffset = 160, VOffset = 416 });
                stMaterialID.Add(new MaterialID() { MName = "mt_se2", MWidth = 64, MHeight = 64, UOffset = 128, VOffset = 256 });
                stMaterialID.Add(new MaterialID() { MName = "mt_sg01", MWidth = 32, MHeight = 32, UOffset = 0, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "mt_sg02", MWidth = 32, MHeight = 32, UOffset = 32, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "mt_sg03", MWidth = 32, MHeight = 32, UOffset = 0, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "mt_sg04", MWidth = 32, MHeight = 32, UOffset = 160, VOffset = 384 });
                stMaterialID.Add(new MaterialID() { MName = "mtcoin", MWidth = 64, MHeight = 64, UOffset = 0, VOffset = 256 });
                stMaterialID.Add(new MaterialID() { MName = "mtwas_e", MWidth = 128, MHeight = 32, UOffset = 0, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "mtwas_ed", MWidth = 128, MHeight = 32, UOffset = 0, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "ncol_gro", MWidth = 64, MHeight = 64, UOffset = 64, VOffset = 384 });
                stMaterialID.Add(new MaterialID() { MName = "ncole01", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 384 });
                stMaterialID.Add(new MaterialID() { MName = "ncole02", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 416 });
                stMaterialID.Add(new MaterialID() { MName = "nivl_gro", MWidth = 64, MHeight = 64, UOffset = 128, VOffset = 384 });
                stMaterialID.Add(new MaterialID() { MName = "nivl_mt", MWidth = 128, MHeight = 64, UOffset = 0, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "nivl_top", MWidth = 32, MHeight = 32, UOffset = 0, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "nivlr", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 384 });
                stMaterialID.Add(new MaterialID() { MName = "port", MWidth = 32, MHeight = 32, UOffset = 96, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "port_d", MWidth = 32, MHeight = 32, UOffset = 160, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "rzclf02", MWidth = 64, MHeight = 64, UOffset = 128, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "rct_gro", MWidth = 64, MHeight = 128, UOffset = 0, VOffset = 416 });
                stMaterialID.Add(new MaterialID() { MName = "riv_cls", MWidth = 64, MHeight = 64, UOffset = 64, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "riv_l1", MWidth = 32, MHeight = 32, UOffset = 96, VOffset = 320 });
                stMaterialID.Add(new MaterialID() { MName = "riv_m", MWidth = 32, MHeight = 32, UOffset = 0, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "rivr", MWidth = 32, MHeight = 32, UOffset = 64, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "rivrclf", MWidth = 64, MHeight = 64, UOffset = 128, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "rivs1", MWidth = 32, MHeight = 32, UOffset = 128, VOffset = 320 });
                stMaterialID.Add(new MaterialID() { MName = "rivshr", MWidth = 64, MHeight = 64, UOffset = 192, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "rivssr", MWidth = 64, MHeight = 32, UOffset = 192, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "rivstrt", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 160 });
                stMaterialID.Add(new MaterialID() { MName = "rm1", MWidth = 32, MHeight = 32, UOffset = 32, VOffset = 288 });
                stMaterialID.Add(new MaterialID() { MName = "rocet", MWidth = 32, MHeight = 32, UOffset = 128, VOffset = 160 });
                stMaterialID.Add(new MaterialID() { MName = "rs_ss", MWidth = 32, MHeight = 32, UOffset = 96, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "sango", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 320 });
                stMaterialID.Add(new MaterialID() { MName = "sango2", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 352 });
                stMaterialID.Add(new MaterialID() { MName = "sango3", MWidth = 32, MHeight = 32, UOffset = 128, VOffset = 384 });
                stMaterialID.Add(new MaterialID() { MName = "sango4", MWidth = 64, MHeight = 64, UOffset = 0, VOffset = 384 });
                stMaterialID.Add(new MaterialID() { MName = "sdun", MWidth = 64, MHeight = 64, UOffset = 0, VOffset = 160 });
                stMaterialID.Add(new MaterialID() { MName = "sdun02", MWidth = 64, MHeight = 64, UOffset = 64, VOffset = 160 });
                stMaterialID.Add(new MaterialID() { MName = "sh1", MWidth = 32, MHeight = 32, UOffset = 32, VOffset = 256 });
                stMaterialID.Add(new MaterialID() { MName = "sh_s1", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 288 });
                stMaterialID.Add(new MaterialID() { MName = "shedge", MWidth = 32, MHeight = 64, UOffset = 160, VOffset = 160 });
                stMaterialID.Add(new MaterialID() { MName = "shlm_1", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 320 });
                stMaterialID.Add(new MaterialID() { MName = "shol", MWidth = 128, MHeight = 128, UOffset = 128, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "shol_s", MWidth = 64, MHeight = 64, UOffset = 192, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "shor", MWidth = 128, MHeight = 128, UOffset = 0, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "shor_s", MWidth = 64, MHeight = 64, UOffset = 128, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "shor_s2", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 416 });
                stMaterialID.Add(new MaterialID() { MName = "shor_v", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "silo", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 32 });
                stMaterialID.Add(new MaterialID() { MName = "slope", MWidth = 128, MHeight = 32, UOffset = 0, VOffset = 384 });
                stMaterialID.Add(new MaterialID() { MName = "snow_es", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 480 });
                stMaterialID.Add(new MaterialID() { MName = "snow_es2", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 480 });
                stMaterialID.Add(new MaterialID() { MName = "snow_es3", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 448 });
                stMaterialID.Add(new MaterialID() { MName = "snw_mt", MWidth = 128, MHeight = 128, UOffset = 0, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "snw_mtd", MWidth = 128, MHeight = 128, UOffset = 128, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "snw_mte", MWidth = 64, MHeight = 32, UOffset = 0, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "snw_mted", MWidth = 64, MHeight = 32, UOffset = 64, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "snw_mts", MWidth = 64, MHeight = 128, UOffset = 64, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "snw_mts2", MWidth = 64, MHeight = 32, UOffset = 128, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "snwfld", MWidth = 64, MHeight = 64, UOffset = 0, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "snwfld_s", MWidth = 64, MHeight = 32, UOffset = 128, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "snwgra", MWidth = 64, MHeight = 64, UOffset = 192, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "snwhm01", MWidth = 32, MHeight = 32, UOffset = 32, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "snwhm02", MWidth = 32, MHeight = 32, UOffset = 32, VOffset = 32 });
                stMaterialID.Add(new MaterialID() { MName = "snwods", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "snwood", MWidth = 64, MHeight = 64, UOffset = 192, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "snwtrk", MWidth = 32, MHeight = 64, UOffset = 96, VOffset = 256 });
                stMaterialID.Add(new MaterialID() { MName = "sse_s1", MWidth = 32, MHeight = 32, UOffset = 32, VOffset = 320 });
                stMaterialID.Add(new MaterialID() { MName = "ssee1", MWidth = 32, MHeight = 32, UOffset = 64, VOffset = 288 });
                stMaterialID.Add(new MaterialID() { MName = "sst1", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 256 });
                stMaterialID.Add(new MaterialID() { MName = "stown_r", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 256 });
                stMaterialID.Add(new MaterialID() { MName = "stw_gro", MWidth = 64, MHeight = 64, UOffset = 0, VOffset = 384 });
                stMaterialID.Add(new MaterialID() { MName = "subrg2", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 160 });
                stMaterialID.Add(new MaterialID() { MName = "susbrg", MWidth = 64, MHeight = 64, UOffset = 192, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "sw_se", MWidth = 64, MHeight = 64, UOffset = 0, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "swclf_l", MWidth = 64, MHeight = 64, UOffset = 64, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "swclf_ld", MWidth = 64, MHeight = 64, UOffset = 192, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "swclf_lg", MWidth = 32, MHeight = 64, UOffset = 0, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "swclf_s", MWidth = 64, MHeight = 32, UOffset = 128, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "swclf_sd", MWidth = 64, MHeight = 32, UOffset = 192, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "swclf_sg", MWidth = 32, MHeight = 32, UOffset = 32, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "swclf_wg", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "swfld_s2", MWidth = 64, MHeight = 32, UOffset = 128, VOffset = 160 });
                stMaterialID.Add(new MaterialID() { MName = "swfld_s3", MWidth = 32, MHeight = 32, UOffset = 160, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "swmd_cg", MWidth = 32, MHeight = 32, UOffset = 128, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "swmd_clf", MWidth = 64, MHeight = 32, UOffset = 64, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "swp1", MWidth = 32, MHeight = 32, UOffset = 0, VOffset = 288 });
                stMaterialID.Add(new MaterialID() { MName = "trk", MWidth = 64, MHeight = 64, UOffset = 128, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "tyo_f", MWidth = 128, MHeight = 128, UOffset = 128, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "tyosnw", MWidth = 64, MHeight = 128, UOffset = 64, VOffset = 384 });
                stMaterialID.Add(new MaterialID() { MName = "uf1", MWidth = 32, MHeight = 32, UOffset = 160, VOffset = 256 });
                stMaterialID.Add(new MaterialID() { MName = "utai01", MWidth = 32, MHeight = 32, UOffset = 32, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "utai02", MWidth = 32, MHeight = 32, UOffset = 224, VOffset = 64 });
                stMaterialID.Add(new MaterialID() { MName = "utai_gro", MWidth = 64, MHeight = 64, UOffset = 128, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "utaimt", MWidth = 32, MHeight = 32, UOffset = 0, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "utaimtd", MWidth = 32, MHeight = 32, UOffset = 96, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "utaimtg", MWidth = 32, MHeight = 32, UOffset = 96, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "wa1", MWidth = 32, MHeight = 32, UOffset = 192, VOffset = 320 });
                stMaterialID.Add(new MaterialID() { MName = "wzs1", MWidth = 32, MHeight = 32, UOffset = 128, VOffset = 288 });
                stMaterialID.Add(new MaterialID() { MName = "wzshr", MWidth = 32, MHeight = 32, UOffset = 160, VOffset = 32 });
                stMaterialID.Add(new MaterialID() { MName = "wzshr2", MWidth = 32, MHeight = 32, UOffset = 32, VOffset = 128 });
                stMaterialID.Add(new MaterialID() { MName = "wzshrs", MWidth = 32, MHeight = 32, UOffset = 32, VOffset = 160 });
                stMaterialID.Add(new MaterialID() { MName = "was", MWidth = 128, MHeight = 128, UOffset = 0, VOffset = 96 });
                stMaterialID.Add(new MaterialID() { MName = "was_d", MWidth = 64, MHeight = 32, UOffset = 0, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "was_g", MWidth = 64, MHeight = 64, UOffset = 0, VOffset = 192 });
                stMaterialID.Add(new MaterialID() { MName = "was_s", MWidth = 128, MHeight = 128, UOffset = 128, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "wasfld", MWidth = 64, MHeight = 64, UOffset = 64, VOffset = 256 });
                stMaterialID.Add(new MaterialID() { MName = "wdedge", MWidth = 64, MHeight = 64, UOffset = 64, VOffset = 160 });
                stMaterialID.Add(new MaterialID() { MName = "we1", MWidth = 32, MHeight = 32, UOffset = 96, VOffset = 256 });
                stMaterialID.Add(new MaterialID() { MName = "we_s1", MWidth = 32, MHeight = 32, UOffset = 160, VOffset = 288 });
                stMaterialID.Add(new MaterialID() { MName = "wedged", MWidth = 32, MHeight = 64, UOffset = 128, VOffset = 160 });
                stMaterialID.Add(new MaterialID() { MName = "wod-e2", MWidth = 32, MHeight = 32, UOffset = 64, VOffset = 224 });
                stMaterialID.Add(new MaterialID() { MName = "wood", MWidth = 64, MHeight = 64, UOffset = 192, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "wood_d", MWidth = 64, MHeight = 64, UOffset = 192, VOffset = 160 });
                stMaterialID.Add(new MaterialID() { MName = "wtrk", MWidth = 32, MHeight = 64, UOffset = 64, VOffset = 96 });
            }
            else if (i_ncols == 3)
            {
                stMaterialID.Add(new MaterialID() { MName = "cltr", MWidth = 128, MHeight = 128, UOffset = 0, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "lake_a", MWidth = 128, MHeight = 256, UOffset = 0, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "rock", MWidth = 256, MHeight = 256, UOffset = 0, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "scave", MWidth = 256, MHeight = 256, UOffset = 0, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "ssand", MWidth = 256, MHeight = 256, UOffset = 0, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "swall02", MWidth = 256, MHeight = 256, UOffset = 0, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "sng01", MWidth = 128, MHeight = 128, UOffset = 0, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "sng02", MWidth = 128, MHeight = 128, UOffset = 0, VOffset = 0 });
            }
            else
            {
                stMaterialID.Add(new MaterialID() { MName = "hokola01", MWidth = 64, MHeight = 64, UOffset = 0, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "hokola02", MWidth = 64, MHeight = 64, UOffset = 0, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "snwfldl", MWidth = 64, MHeight = 64, UOffset = 0, VOffset = 0 });
                stMaterialID.Add(new MaterialID() { MName = "snwfld2", MWidth = 64, MHeight = 64, UOffset = 0, VOffset = 0 });
            }

        }

        public static int writeMatLib(string str_mtlfilename, string str_texturepath, string str_imgformat, string str_blockspath)
        {
            int i_result = 0;
            string[] str_splitMatBlock;

            if (str_blockspath != "") str_mtlfilename = str_blockspath + "\\" + str_mtlfilename;

            try
            {
                // here we will put in .mtl format (for .obj format) the materials list
                using (StreamWriter fs_mtl = new StreamWriter(str_mtlfilename))
                {

                    // some material file header
                    fs_mtl.WriteLine("# Gaia MTL file exported from: " + str_filename.ToUpper() + ".MAP");

                    // write each material
                    if (b_fullmaterial)
                    {
                        // write each material
                        foreach (string matBlock in Map2Obj.lst_MatBlock[0])
                        {
                            str_splitMatBlock = matBlock.Split(':');
                            fs_mtl.WriteLine("newmtl " + matBlock.Replace(':', '_'));

                            fs_mtl.WriteLine("map_Ka " + str_texturepath + "\\" + str_splitMatBlock[0] + "." + str_imgformat);
                            fs_mtl.WriteLine("map_Kd " + str_texturepath + "\\" + str_splitMatBlock[0] + "." + str_imgformat);

                            fs_mtl.WriteLine("");
                        }
                    }
                    else
                    {
                        foreach (MaterialID mid in stMaterialID)
                        {
                            // put the materials
                            fs_mtl.WriteLine("newmtl " + mid.MName);
                            fs_mtl.WriteLine("map_Ka " + str_texturepath + "\\" + mid.MName + "." + str_imgformat);
                            fs_mtl.WriteLine("map_Kd " + str_texturepath + "\\" + mid.MName + "." + str_imgformat);
                            fs_mtl.WriteLine("");
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

        public static int writeMatLibBlocks(string str_filename, string str_texturepath, string str_imgformat, string str_blockspath)
        {
            int i_result = 0;
            int i_block;
            string[] str_splitMatBlock;

            try
            {
                // here we will put in .mtl format (for .obj format) the materials list
                for (i_block = 0; i_block < i_nblocks; i_block++)
                {

                    using (StreamWriter fs_mtl = new StreamWriter(str_blockspath + "\\" + str_filename + "_" + i_block.ToString("00") + ".mtl"))
                    {

                        // some material file header
                        fs_mtl.WriteLine("# Gaia .mtl creation for: " + str_filename + "_" + i_block.ToString("00") + ".mtl");

                                                 
                        // write each material
                        foreach (string matBlock in Map2Obj.lst_MatBlock[i_block])
                        {
                            str_splitMatBlock = matBlock.Split(':');
                            fs_mtl.WriteLine("newmtl " + matBlock.Replace(':', '_'));

                            fs_mtl.WriteLine("map_Ka " + str_texturepath + "\\" + str_splitMatBlock[0] + "." + str_imgformat);
                            fs_mtl.WriteLine("map_Kd " + str_texturepath + "\\" + str_splitMatBlock[0] + "." + str_imgformat);

                            fs_mtl.WriteLine("");
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
    }


    public static class Walkmap
    {

        public struct WalkmapID
        {
            public string WName;
        }

        public static List<WalkmapID> stWalkmapID = new List<WalkmapID>();


        public static void PopulateWalkmapID()
        {
            stWalkmapID.Add(new WalkmapID() { WName = "GRASS" });
            stWalkmapID.Add(new WalkmapID() { WName = "FOREST" });
            stWalkmapID.Add(new WalkmapID() { WName = "MOUNTAIN" });
            stWalkmapID.Add(new WalkmapID() { WName = "SEA" });
            stWalkmapID.Add(new WalkmapID() { WName = "RIVERCROSS" });
            stWalkmapID.Add(new WalkmapID() { WName = "RIVER" });
            stWalkmapID.Add(new WalkmapID() { WName = "WATER" });
            stWalkmapID.Add(new WalkmapID() { WName = "SWAMP" });
            stWalkmapID.Add(new WalkmapID() { WName = "DESERT" });
            stWalkmapID.Add(new WalkmapID() { WName = "WASTELAND" });
            stWalkmapID.Add(new WalkmapID() { WName = "SNOW" });
            stWalkmapID.Add(new WalkmapID() { WName = "RIVERSIDE" });
            stWalkmapID.Add(new WalkmapID() { WName = "CLIFF" });
            stWalkmapID.Add(new WalkmapID() { WName = "CORELBRDG" });
            stWalkmapID.Add(new WalkmapID() { WName = "WUTAIBRDG" });
            stWalkmapID.Add(new WalkmapID() { WName = "UNUSED1" });
            stWalkmapID.Add(new WalkmapID() { WName = "HILLSIDE" });
            stWalkmapID.Add(new WalkmapID() { WName = "BEACH" });
            stWalkmapID.Add(new WalkmapID() { WName = "SUBPEN" });
            stWalkmapID.Add(new WalkmapID() { WName = "CANYON" });
            stWalkmapID.Add(new WalkmapID() { WName = "MNTPASS" });
            stWalkmapID.Add(new WalkmapID() { WName = "UNKNOWN1" });
            stWalkmapID.Add(new WalkmapID() { WName = "WATERFALL" });
            stWalkmapID.Add(new WalkmapID() { WName = "UNUSED2" });
            stWalkmapID.Add(new WalkmapID() { WName = "GLDDESERT" });
            stWalkmapID.Add(new WalkmapID() { WName = "JUNGLE" });
            stWalkmapID.Add(new WalkmapID() { WName = "SEA2" });
            stWalkmapID.Add(new WalkmapID() { WName = "NCAVE" });
            stWalkmapID.Add(new WalkmapID() { WName = "DSRTBORDER" });
            stWalkmapID.Add(new WalkmapID() { WName = "BRDGHEAD" });
            stWalkmapID.Add(new WalkmapID() { WName = "BACK" });
            stWalkmapID.Add(new WalkmapID() { WName = "UNUSED3" });
        }
    }
}
