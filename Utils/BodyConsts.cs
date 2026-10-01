using System.Collections.Frozen;
using System.Collections.Immutable;

namespace DanielleDotnet.Utils;

public static class BodyConsts
{
    public static readonly string PREFIX = "objects/characters/humans/";
    public static readonly string MALE_BODY_TYPE = "objects/characters/humans/genmale/genmale.chr";
    public static readonly string FEMALE_BODY_TYPE = "objects/characters/humans/genfemale/genfemale.chr";
    public static readonly string LARGE_MALE_BODY_TYPE = "objects/characters/humans/largemale/largemale.chr";

    public static readonly string LARGE_MALE_ATTACHMENTS = "objects/characters/humans/largemale/largemale_attachments.cdf";
    public static readonly string MALE_ATTACHMENTS = "objects/characters/humans/genmale/genmale_attachments.cdf";
    public static readonly string FEMALE_ATTACHMENTS = "objects/characters/humans/genfemale/genfemale_attachments.cdf";

    public static readonly ImmutableArray<string> LARGE_MALE_MTLS = ImmutableArray.Create("corporate/corporate_largemalebody_variant",
        "corporate/corporate_largemalebody01");
    public static readonly ImmutableArray<string> LARGE_MALE_HEADS = ImmutableArray.Create("Luka/Luka_LargeMaleHead01",
        "alexlikarl/alexlikarl_largemalehead01", "alexlikarl/alexlikarl_largemalehead01_variant");
    public static readonly ImmutableArray<string> ALEX_HAIRS = ImmutableArray.Create("AlexLiKarl/AlexLiKarl_LargeMaleHead01_Hair01",
        "alexlikarl_largemalehead01_variant_hair01");

    public enum FemaleBody
    {
        CORPORATE, MECHANIC, PLUMBER, SCIENTIST, SECURITY, GLOVELESS
        // TODO: Add labcoat, morgan shift
    }

    public enum MaleBody
    {
        CORPORATE, DAHL, LABCOAT, MECHANIC, SCIENTIST, SECURITY, VOLUNTEER, GLOVELESS
        // TODO: Add player models, morgan shift
    }

    // TODO: Add wounded alex variant
    public enum LargeMaleBody
    {
        ALEX, ALEX_VARIANT
    }

    public enum FemaleHead
    {
        HEAD01, HEAD02, HEAD02_PSYCHOSCOPE, HEAD03, HEAD04, HEAD05, HEAD06, MIKHAILA, MORGAN, SARA, DANIELLE, HELMET, HUSK
        // TODO: Add prototype psychoscope
    }

    public enum MaleHead
    {
        DAHL, CALVINO, IGWE, HEAD01, HEAD01_PSYCHOSCOPE, HEAD01_PSYCHO, HEAD02, HEAD03, HEAD03_PSYCHOSCOPE, HEAD04, HEAD05,
        HEAD06, INGRAM, MORGAN, VOLUNTEER01, VOLUNTEER02, VOLUNTEER03, VOLUNTEER04, HELMET, HUSK
    }

    public enum LargeMaleHead
    {
        ALEX, ALEX_VARIANT, LUKA
    }

    public enum FemaleHair
    {
        HEAD01_HAIR02, HEAD01_HAIR03, HEAD01_HAIR05, HEAD02_HAIR02, HEAD02_HAIR03, HEAD02_HAIR04, HEAD02_HAIR05,
        HEAD03_HAIR02, HEAD03_HAIR03, HEAD03_HAIR04, HEAD03_HAIR05, HEAD04_HAIR03, HEAD04_HAIR04, HEAD04_HAIR05,
        HEAD05_HAIR03, HEAD05_HAIR05,
        // HEAD06_HAIR02, // Unused
        HEAD06_HAIR03, HEAD06_HAIR05, HEAD06_HAIR06, MIKHAILA, MORGAN, SARA
    }

    public enum FemaleHairType
    {
        HAIR02, HAIR03, HAIR04, HAIR05, HAIR06, MIKHAILA, MORGAN, SARA
    }

    public enum FemaleHairColor
    {
        // HAIR01_BLACK, // Unused
        HAIR02_BLACK, HAIR02_BLONDE, HAIR02_BROWN, HAIR02_GRAY, HAIR02_RED, HAIR03, HAIR03_BLACK, HAIR03_BLONDE,
        HAIR03_BROWN, HAIR03_RED, HAIR04_BLACK,
        // HAIR04_BLONDE, // Unused
        HAIR04_BROWN, HAIR04_RED, HAIR05_BLACK, HAIR05_BLONDE, HAIR05_BROWN, HAIR05_RED, HAIR06, MIKHAILA, MORGAN, SARA
    }

    public enum MaleHairType
    {
        HAIR01, HAIR03, HAIR04, HAIR05, HAIR06, VOLUNTEER, MORGAN, DAHL, IGWE, CALVINO
    }

    public enum MaleHair
    {
        HEAD01_HAIR01, HEAD01_HAIR03, HEAD01_HAIR05, HEAD02_HAIR01, HEAD02_HAIR03, HEAD02_HAIR05, HEAD03_HAIR03,
        HEAD03_HAIR05, HEAD03_HAIR06, HEAD04_HAIR04, HEAD05_HAIR01, HEAD05_HAIR05, HEAD05_HAIR06, HEAD06_HAIR01,
        HEAD06_HAIR05, HEAD06_HAIR06, VOLUNTEER01, VOLUNTEER02, VOLUNTEER03, MORGAN, DAHL, IGWE, CALVINO
    }

    public enum MaleHairColor
    {
        HAIR01, HAIR01_BLACK, HAIR01_BLONDE, HAIR01_BROWN, HAIR01_RED, HAIR03, HAIR03_BLACK, HAIR03_BLONDE, HAIR03_BROWN,
        HAIR03_RED, HAIR04, HAIR05, HAIR05_BLACK, HAIR05_BLONDE, HAIR05_BROWN, HAIR05_RED, HAIR06, HAIR06_BLACK,
        HAIR06_BLONDE, MORGAN, DAHL, IGWE, CALVINO, VOLUNTEER, VOLUNTEER_BROWN
    }

    public enum LargeMaleHair
    {
        ALEX, ALEX_VARIANT, LUKA
    }

    public static readonly ImmutableHashSet<MaleHead> HEADS_THAT_CAN_BE_BALD = ImmutableHashSet.Create(MaleHead.HEAD04, MaleHead.INGRAM,
        MaleHead.VOLUNTEER01, MaleHead.VOLUNTEER02, MaleHead.VOLUNTEER03);


    public static readonly FrozenDictionary<MaleHead, ImmutableHashSet<MaleHair>> MALE_HEAD_HAIR_COMPATIBILITY = new Dictionary<MaleHead, ImmutableHashSet<MaleHair>>()
    {
        [MaleHead.HEAD01] =
        [
            MaleHair.HEAD01_HAIR01,
            MaleHair.HEAD01_HAIR03,
            MaleHair.HEAD01_HAIR05
        ],
        [MaleHead.HEAD02] =
        [
            MaleHair.HEAD02_HAIR01,
            MaleHair.HEAD02_HAIR03,
            MaleHair.HEAD02_HAIR05
        ],
        [MaleHead.HEAD03] =
        [
            MaleHair.HEAD03_HAIR03,
            MaleHair.HEAD03_HAIR05,
            MaleHair.HEAD03_HAIR06
        ],
        [MaleHead.HEAD04] =
        [
            MaleHair.HEAD04_HAIR04
        ],
        [MaleHead.HEAD05] =
        [
            MaleHair.HEAD05_HAIR01,
            MaleHair.HEAD05_HAIR05,
            MaleHair.HEAD05_HAIR06
        ],
        [MaleHead.HEAD06] =
        [
            MaleHair.HEAD06_HAIR01,
            MaleHair.HEAD06_HAIR05,
            MaleHair.HEAD06_HAIR06
        ],
        [MaleHead.MORGAN] =
        [
            MaleHair.MORGAN
        ],
        [MaleHead.DAHL] =
        [
            MaleHair.DAHL,
            MaleHair.MORGAN
        ],
        [MaleHead.CALVINO] =
        [
            MaleHair.CALVINO
        ],
        [MaleHead.IGWE] =
        [
            MaleHair.IGWE,
            MaleHair.HEAD02_HAIR01,
            MaleHair.HEAD02_HAIR03,
            MaleHair.HEAD02_HAIR05
        ],
        [MaleHead.INGRAM] =
        [
            MaleHair.MORGAN
        ],
        [MaleHead.VOLUNTEER01] =
        [
            MaleHair.VOLUNTEER01
        ],
        [MaleHead.VOLUNTEER02] =
        [
            MaleHair.VOLUNTEER02
        ],
        [MaleHead.VOLUNTEER03] =
        [
            MaleHair.VOLUNTEER03
        ]
    }.ToFrozenDictionary(); // Creates overhead on creation but results in faster lookups

    public static readonly FrozenDictionary<MaleHairType, ImmutableHashSet<MaleHairColor>>
        MALE_HAIR_COLOR_COMPATIBILITY = new Dictionary<MaleHairType, ImmutableHashSet<MaleHairColor>>()
        {
            [MaleHairType.HAIR01] =
            [
                MaleHairColor.HAIR01,
                MaleHairColor.HAIR01_BLACK,
                MaleHairColor.HAIR01_BLONDE,
                MaleHairColor.HAIR01_BROWN,
                MaleHairColor.HAIR01_RED
            ],
            [MaleHairType.HAIR03] =
            [
                MaleHairColor.HAIR03,
                MaleHairColor.HAIR03_BLACK,
                MaleHairColor.HAIR03_BLONDE,
                MaleHairColor.HAIR03_BROWN,
                MaleHairColor.HAIR03_RED
            ],
            [MaleHairType.HAIR04] =
            [
                MaleHairColor.HAIR04
            ],
            [MaleHairType.HAIR05] =
            [
                MaleHairColor.HAIR05,
                MaleHairColor.HAIR05_BLACK,
                MaleHairColor.HAIR05_BLONDE,
                MaleHairColor.HAIR05_BROWN,
                MaleHairColor.HAIR05_RED
            ],
            [MaleHairType.HAIR06] =
            [
                MaleHairColor.HAIR06,
                MaleHairColor.HAIR06_BLACK,
                MaleHairColor.HAIR06_BLONDE
            ],
            [MaleHairType.MORGAN] = [MaleHairColor.MORGAN],
            [MaleHairType.DAHL] = [MaleHairColor.DAHL],
            [MaleHairType.IGWE] = [MaleHairColor.IGWE],
            [MaleHairType.CALVINO] = [MaleHairColor.CALVINO],
            [MaleHairType.VOLUNTEER] =
            [
                MaleHairColor.VOLUNTEER,
                MaleHairColor.VOLUNTEER_BROWN
            ]
        }.ToFrozenDictionary();

    public static readonly FrozenDictionary<FemaleHead, ImmutableHashSet<FemaleHair>>
        FEMALE_HEAD_HAIR_COMPATIBILITY = new Dictionary<FemaleHead, ImmutableHashSet<FemaleHair>>()
        {
            [FemaleHead.HEAD01] =
            [
                FemaleHair.HEAD01_HAIR02,
                FemaleHair.HEAD01_HAIR03,
                FemaleHair.HEAD01_HAIR05
            ],
            [FemaleHead.HEAD02] =
            [
                FemaleHair.HEAD02_HAIR02,
                FemaleHair.HEAD02_HAIR03,
                FemaleHair.HEAD02_HAIR04,
                FemaleHair.HEAD02_HAIR05,
                FemaleHair.MORGAN
            ],
            [FemaleHead.HEAD03] =
            [
                FemaleHair.HEAD03_HAIR02,
                FemaleHair.HEAD03_HAIR03,
                FemaleHair.HEAD03_HAIR04,
                FemaleHair.HEAD03_HAIR05
            ],
            [FemaleHead.HEAD04] =
            [
                FemaleHair.HEAD04_HAIR03,
                FemaleHair.HEAD04_HAIR04,
                FemaleHair.HEAD04_HAIR05,
                FemaleHair.HEAD04_HAIR05
            ],
            [FemaleHead.HEAD05] =
            [
                FemaleHair.HEAD05_HAIR03,
                FemaleHair.HEAD05_HAIR05
            ],
            [FemaleHead.HEAD06] =
            [
                FemaleHair.HEAD06_HAIR03,
                FemaleHair.HEAD06_HAIR05
            ],
            [FemaleHead.MIKHAILA] =
            [
                FemaleHair.MIKHAILA,
                FemaleHair.HEAD02_HAIR02,
                FemaleHair.HEAD02_HAIR03,
                FemaleHair.HEAD02_HAIR04,
                FemaleHair.HEAD02_HAIR05,
                FemaleHair.SARA
            ],
            [FemaleHead.MORGAN] =
            [
                FemaleHair.MORGAN,
                FemaleHair.HEAD02_HAIR02,
                FemaleHair.HEAD02_HAIR03,
                FemaleHair.HEAD02_HAIR04,
                FemaleHair.HEAD02_HAIR05,
                FemaleHair.SARA
            ],
            [FemaleHead.SARA] =
            [
                FemaleHair.SARA,
                FemaleHair.HEAD02_HAIR02,
                FemaleHair.HEAD02_HAIR03,
                FemaleHair.HEAD02_HAIR04,
                FemaleHair.HEAD02_HAIR05,
                FemaleHair.MORGAN,
                FemaleHair.MIKHAILA
            ]
        }.ToFrozenDictionary();

    public static readonly FrozenDictionary<FemaleHairType, ImmutableHashSet<FemaleHairColor>>
        FEMALE_HAIR_COLOR_COMPATIBILITY = new Dictionary<FemaleHairType, ImmutableHashSet<FemaleHairColor>>()
        {
            [FemaleHairType.HAIR02] =
            [
                FemaleHairColor.HAIR02_BLACK,
                FemaleHairColor.HAIR02_BLONDE,
                FemaleHairColor.HAIR02_BROWN,
                FemaleHairColor.HAIR02_GRAY,
                FemaleHairColor.HAIR02_RED
            ],
            [FemaleHairType.HAIR03] =
            [
                FemaleHairColor.HAIR03,
                FemaleHairColor.HAIR03_BLACK,
                FemaleHairColor.HAIR03_BLONDE,
                FemaleHairColor.HAIR03_BROWN,
                FemaleHairColor.HAIR03_RED
            ],
            [FemaleHairType.HAIR04] =
            [
                FemaleHairColor.HAIR04_BLACK,
                FemaleHairColor.HAIR04_BROWN,
                FemaleHairColor.HAIR04_RED
            ],
            [FemaleHairType.HAIR05] =
            [
                FemaleHairColor.HAIR05_BLACK,
                FemaleHairColor.HAIR05_BLONDE,
                FemaleHairColor.HAIR05_BROWN,
                FemaleHairColor.HAIR05_RED
            ],
            [FemaleHairType.HAIR06] = [FemaleHairColor.HAIR06],
            [FemaleHairType.MIKHAILA] = [FemaleHairColor.MIKHAILA],
            [FemaleHairType.MORGAN] = [FemaleHairColor.MORGAN],
            [FemaleHairType.SARA] = [FemaleHairColor.SARA]
        }.ToFrozenDictionary();

    public static readonly FrozenDictionary<FemaleBody, string> FEMALE_BODIES_MAP = new Dictionary<FemaleBody, string>()
    {
        [FemaleBody.CORPORATE] = "corporate/corporate_genfemalebody01",
        [FemaleBody.MECHANIC] = "mechanic/mechanic_genfemalebody01",
        [FemaleBody.SCIENTIST] = "scientist/scientist_genfemalebody01",
        [FemaleBody.SECURITY] = "security/security_genfemalebody01",
        [FemaleBody.GLOVELESS] = "morgankarl/morgankarlgenderselect_genfemale",
        [FemaleBody.PLUMBER] = "plumber/plumber_genfemalebody01"
    }.ToFrozenDictionary();

    public static readonly FrozenDictionary<MaleBody, string> MALE_BODIES_MAP = new Dictionary<MaleBody, string>()
    {
        [MaleBody.CORPORATE] = "corporate/corporate_genmalebody01",
        [MaleBody.DAHL] = "dahl/dahl_genmalebody01",
        [MaleBody.LABCOAT] = "labcoat/labcoat_genmalebody01",
        [MaleBody.MECHANIC] = "mechanic/mechanic_genmalebody01",
        [MaleBody.SCIENTIST] = "scientist/scientist_genmalebody01",
        [MaleBody.SECURITY] = "security/security_genmalebody01",
        [MaleBody.VOLUNTEER] = "volunteer/volunteer_genmalebody01",
        [MaleBody.GLOVELESS] = "morgankarl/morgankarlgenderselect_genmale"
    }.ToFrozenDictionary();

    public static readonly FrozenDictionary<FemaleHead, string> FEMALE_HEADS_MAP = new Dictionary<FemaleHead, string>()
    {
        [FemaleHead.HEAD01] = "genfemale/genfemale_head01",
        [FemaleHead.HEAD02] = "genfemale/genfemale_head02",
        [FemaleHead.HEAD02_PSYCHOSCOPE] = "genfemale/genfemale_head02_psychoscope",
        [FemaleHead.HEAD03] = "genfemale/genfemale_head03",
        [FemaleHead.HEAD04] = "genfemale/genfemale_head04",
        [FemaleHead.HEAD05] = "genfemale/genfemale_head05",
        [FemaleHead.HEAD06] = "genfemale/genfemale_head06",
        [FemaleHead.MIKHAILA] = "mikhailailyushin/mikhailailyushin_genfemalehead01",
        [FemaleHead.MORGAN] = "morgankarl/morgankarl_genfemalehead01",
        [FemaleHead.SARA] = "saraelazar/saraelazar_genfemalehead01",
        [FemaleHead.DANIELLE] = "objects/accessories/breather/breather3p/breather3p_genfemale01",
        [FemaleHead.HELMET] = "scientist/scientist_genfemalehead01",
        [FemaleHead.HUSK] = "husk/husk_genfemalehead01"
    }.ToFrozenDictionary();

    public static readonly FrozenDictionary<MaleHead, string> MALE_HEADS_MAP = new Dictionary<MaleHead, string>()
    {
        [MaleHead.DAHL] = "dahl/dahl_genmalehead01",
        [MaleHead.CALVINO] = "drcalvino/drcalvino_genmalehead01",
        [MaleHead.IGWE] = "drigwe/igwe_genmalehead01",
        [MaleHead.HEAD01] = "genmale/genmale_head01",
        [MaleHead.HEAD01_PSYCHOSCOPE] = "genmale/genmale_head01_psychoscope",
        [MaleHead.HEAD01_PSYCHO] = "genmale/genmale_head01_psycho",
        [MaleHead.HEAD02] = "genmale/genmale_head02",
        [MaleHead.HEAD03] = "genmale/genmale_head03",
        [MaleHead.HEAD03_PSYCHOSCOPE] = "genmale/genmale_head03_psychoscope",
        [MaleHead.HEAD04] = "genmale/genmale_head04",
        [MaleHead.HEAD05] = "genmale/genmale_head05",
        [MaleHead.HEAD06] = "genmale/genmale_head06",
        [MaleHead.MORGAN] = "morgankarl/morgankarl_genmalehead01",
        [MaleHead.VOLUNTEER01] = "volunteer/volunteer_genmalehead01",
        [MaleHead.VOLUNTEER02] = "volunteer/volunteer_genmalehead02",
        [MaleHead.VOLUNTEER03] = "volunteer/volunteer_genmalehead03",
        [MaleHead.VOLUNTEER04] = "volunteer/volunteer02_genmalehead01",
        [MaleHead.HELMET] = "scientist/scientist_genmalehead01",
        [MaleHead.HUSK] = "husk/husk_genmalehead01",
        [MaleHead.INGRAM] = "aaroningram/aaroningram_genmale_head01"
    }.ToFrozenDictionary();

    public static readonly FrozenDictionary<LargeMaleHead, string> LARGE_MALE_HEADS_MAP = new Dictionary<LargeMaleHead, string>()
    {
        [LargeMaleHead.ALEX] = "alexlikarl/alexlikarl_largemalehead01",
        [LargeMaleHead.ALEX_VARIANT] = "alexlikarl/alexlikarl_largemalehead01_variant",
        [LargeMaleHead.LUKA] = "luka/luka_largemalehead01"
    }.ToFrozenDictionary();

    public static readonly FrozenDictionary<FemaleHair, string> FEMALE_HAIRS_MAP = new Dictionary<FemaleHair, string>()
    {
        [FemaleHair.HEAD01_HAIR02] = "genfemale/genfemale_head01_hair02",
        [FemaleHair.HEAD01_HAIR03] = "genfemale/genfemale_head01_hair03",
        [FemaleHair.HEAD01_HAIR05] = "genfemale/genfemale_head01_hair05",
        [FemaleHair.HEAD02_HAIR02] = "genfemale/genfemale_head02_hair02",
        [FemaleHair.HEAD02_HAIR03] = "genfemale/genfemale_head02_hair03",
        [FemaleHair.HEAD02_HAIR04] = "genfemale/genfemale_head02_hair04",
        [FemaleHair.HEAD02_HAIR05] = "genfemale/genfemale_head02_hair05",
        [FemaleHair.HEAD03_HAIR02] = "genfemale/genfemale_head03_hair02",
        [FemaleHair.HEAD03_HAIR03] = "genfemale/genfemale_head03_hair03",
        [FemaleHair.HEAD03_HAIR04] = "genfemale/genfemale_head03_hair04",
        [FemaleHair.HEAD03_HAIR05] = "genfemale/genfemale_head03_hair05",
        [FemaleHair.HEAD04_HAIR03] = "genfemale/genfemale_head04_hair03",
        [FemaleHair.HEAD04_HAIR04] = "genfemale/genfemale_head04_hair04",
        [FemaleHair.HEAD04_HAIR05] = "genfemale/genfemale_head04_hair05",
        [FemaleHair.HEAD05_HAIR03] = "genfemale/genfemale_head05_hair03",
        [FemaleHair.HEAD05_HAIR05] = "genfemale/genfemale_head05_hair05",
        [FemaleHair.HEAD06_HAIR03] = "genfemale/genfemale_head06_hair03",
        [FemaleHair.HEAD06_HAIR05] = "genfemale/genfemale_head06_hair05",
        [FemaleHair.HEAD06_HAIR06] = "genfemale/genfemale_head06_hair06",
        [FemaleHair.MIKHAILA] = "mikhailailyushin/mikhailailyushin_genfemalehead01_hair01",
        [FemaleHair.MORGAN] = "morgankarl/morgankarl_genfemalehead01_hair01",
        [FemaleHair.SARA] = "saraelazar/saraelazar_genfemalehead01_hair01"
    }.ToFrozenDictionary();

    public static readonly FrozenDictionary<FemaleHairColor, string> FEMALE_HAIR_COLORS_MAP = new Dictionary<FemaleHairColor, string>()
    {
        [FemaleHairColor.HAIR02_BLACK] = "genfemale/genfemale_hair02_black",
        [FemaleHairColor.HAIR02_BLONDE] = "genfemale/genfemale_hair02_blonde",
        [FemaleHairColor.HAIR02_BROWN] = "genfemale/genfemale_hair02_brown",
        [FemaleHairColor.HAIR02_GRAY] = "genfemale/genfemale_hair02_gray",
        [FemaleHairColor.HAIR02_RED] = "genfemale/genfemale_hair02_red",
        [FemaleHairColor.HAIR03] = "genfemale/genfemale_hair03",
        [FemaleHairColor.HAIR03_BLACK] = "genfemale/genfemale_hair03_black",
        [FemaleHairColor.HAIR03_BLONDE] = "genfemale/genfemale_hair03_blonde",
        [FemaleHairColor.HAIR03_BROWN] = "genfemale/genfemale_hair03_brown",
        [FemaleHairColor.HAIR03_RED] = "genfemale/genfemale_hair03_red",
        [FemaleHairColor.HAIR04_BLACK] = "genfemale/genfemale_hair04_black",
        [FemaleHairColor.HAIR04_BROWN] = "genfemale/genfemale_hair04_brown",
        [FemaleHairColor.HAIR04_RED] = "genfemale/genfemale_hair04_red",
        [FemaleHairColor.HAIR05_BLACK] = "genfemale/genfemale_hair05_black",
        [FemaleHairColor.HAIR05_BLONDE] = "genfemale/genfemale_hair05_blonde",
        [FemaleHairColor.HAIR05_BROWN] = "genfemale/genfemale_hair05_brown",
        [FemaleHairColor.HAIR05_RED] = "genfemale/genfemale_hair05_red",
        [FemaleHairColor.HAIR06] = "genfemale/genfemale_hair06",
        [FemaleHairColor.MIKHAILA] = "mikhailailyushin/mikhailailyushin_genfemalehead01_hair01",
        [FemaleHairColor.MORGAN] = "morgankarl/morgankarl_genfemalehead01_hair01",
        [FemaleHairColor.SARA] = "saraelazar/saraelazar_genfemalehead01_hair01"
    }.ToFrozenDictionary();

    public static readonly FrozenDictionary<MaleHair, string> MALE_HAIRS_MAP = new Dictionary<MaleHair, string>()
    {
        [MaleHair.HEAD01_HAIR01] = "genmale/genmale_head01_hair01",
        [MaleHair.HEAD01_HAIR03] = "genmale/genmale_head01_hair03",
        [MaleHair.HEAD01_HAIR05] = "genmale/genmale_head01_hair05",
        [MaleHair.HEAD02_HAIR01] = "genmale/genmale_head02_hair01",
        [MaleHair.HEAD02_HAIR03] = "genmale/genmale_head02_hair03",
        [MaleHair.HEAD02_HAIR05] = "genmale/genmale_head02_hair05",
        [MaleHair.HEAD03_HAIR03] = "genmale/genmale_head03_hair03",
        [MaleHair.HEAD03_HAIR05] = "genmale/genmale_head03_hair05",
        [MaleHair.HEAD03_HAIR06] = "genmale/genmale_head03_hair06",
        [MaleHair.HEAD04_HAIR04] = "genmale/genmale_head04_hair04",
        [MaleHair.HEAD05_HAIR01] = "genmale/genmale_head05_hair01",
        [MaleHair.HEAD05_HAIR05] = "genmale/genmale_head05_hair05",
        [MaleHair.HEAD05_HAIR06] = "genmale/genmale_head05_hair06",
        [MaleHair.HEAD06_HAIR01] = "genmale/genmale_head06_hair01",
        [MaleHair.HEAD06_HAIR05] = "genmale/genmale_head06_hair05",
        [MaleHair.HEAD06_HAIR06] = "genmale/genmale_head06_hair06",
        [MaleHair.VOLUNTEER01] = "volunteer/volunteer_genmalehead01_hair01",
        [MaleHair.VOLUNTEER02] = "volunteer/volunteer_genmalehead02_hair01",
        [MaleHair.VOLUNTEER03] = "volunteer/volunteer_genmalehead03_hair01",
        [MaleHair.MORGAN] = "morgankarl/morgankarl_genmalehead01_hair01",
        [MaleHair.DAHL] = "dahl/dahl_genmalehead01_hair01",
        [MaleHair.IGWE] = "drigwe/igwe_genmalehead01_hair01",
        [MaleHair.CALVINO] = "drcalvino/drcalvino_genmalehead01_hair01"
    }.ToFrozenDictionary();

    public static readonly FrozenDictionary<MaleHairColor, string> MALE_HAIR_COLORS_MAP = new Dictionary<MaleHairColor, string>()
    {
        [MaleHairColor.HAIR01] = "genmale/genmale_hair01",
        [MaleHairColor.HAIR01_BLACK] = "genmale/genmale_hair01_black",
        [MaleHairColor.HAIR01_BLONDE] = "genmale/genmale_hair01_blonde",
        [MaleHairColor.HAIR01_BROWN] = "genmale/genmale_hair01_brown",
        [MaleHairColor.HAIR01_RED] = "genmale/genmale_hair01_red",
        [MaleHairColor.HAIR03] = "genmale/genmale_hair03",
        [MaleHairColor.HAIR03_BLACK] = "genmale/genmale_hair03_black",
        [MaleHairColor.HAIR03_BLONDE] = "genmale/genmale_hair03_blonde",
        [MaleHairColor.HAIR03_BROWN] = "genmale/genmale_hair03_brown",
        [MaleHairColor.HAIR03_RED] = "genmale/genmale_hair03_red",
        [MaleHairColor.HAIR04] = "genmale/genmale_hair04",
        [MaleHairColor.HAIR05] = "genmale/genmale_hair05",
        [MaleHairColor.HAIR05_BLACK] = "genmale/genmale_hair05_black",
        [MaleHairColor.HAIR05_BLONDE] = "genmale/genmale_hair05_blonde",
        [MaleHairColor.HAIR05_BROWN] = "genmale/genmale_hair05_brown",
        [MaleHairColor.HAIR05_RED] = "genmale/genmale_hair05_red",
        [MaleHairColor.HAIR06] = "genmale/genmale_hair06",
        [MaleHairColor.HAIR06_BLACK] = "genmale/genmale_hair06_black",
        [MaleHairColor.HAIR06_BLONDE] = "genmale/genmale_hair06_blonde",
        [MaleHairColor.MORGAN] = "morgankarl/morgankarl_genmalehead01_hair01",
        [MaleHairColor.DAHL] = "dahl/dahl_genmalehead01_hair01",
        [MaleHairColor.IGWE] = "drigwe/igwe_genmalehead01_hair01",
        [MaleHairColor.CALVINO] = "drcalvino/dr_calvino_genmalehead01_hair",
        [MaleHairColor.VOLUNTEER] = "volunteer/volunteer_hair01",
        [MaleHairColor.VOLUNTEER_BROWN] = "volunteer/volunteer_hair01_brown"
    }.ToFrozenDictionary();

    public static readonly FrozenDictionary<LargeMaleHair, string> LARGE_MALE_HAIRS_MAP = new Dictionary<LargeMaleHair, string>()
    {
        [LargeMaleHair.ALEX] = "alexlikarl/alexlikarl_largemalehead01_hair01",
        [LargeMaleHair.ALEX_VARIANT] = "alexlikarl/alexlikarl_largemalehead01_variant_hair01",
        [LargeMaleHair.LUKA] = "luka/luka_largemalehead01_hair01"
    }.ToFrozenDictionary();

    public static readonly string LARGE_MALE_BODY_MODEL = "corporate/corporate_largemalebody01";

    public static readonly ImmutableArray<FemaleBody> SUPPORTED_FEMALE_BODIES = ImmutableArray.Create(FemaleBody.CORPORATE, FemaleBody.MECHANIC,
        FemaleBody.PLUMBER, FemaleBody.SCIENTIST, FemaleBody.SECURITY, FemaleBody.GLOVELESS);
    public static readonly ImmutableArray<int> FEMALE_BODIES_WEIGHTS = ImmutableArray.Create(5, 5, 1, 5, 5, 1);

    public static readonly ImmutableArray<MaleBody> SUPPORTED_MALE_BODIES = ImmutableArray.Create(MaleBody.CORPORATE, MaleBody.DAHL, MaleBody.LABCOAT,
        MaleBody.MECHANIC, MaleBody.SCIENTIST, MaleBody.SECURITY, MaleBody.VOLUNTEER, MaleBody.GLOVELESS);
    public static readonly ImmutableArray<int> MALE_BODIES_WEIGHTS = ImmutableArray.Create(5, 1, 5, 5, 5, 5, 5, 1);

    public static readonly ImmutableArray<LargeMaleBody> SUPPORTED_LARGE_MALE_BODIES = ImmutableArray.Create(LargeMaleBody.ALEX, LargeMaleBody.ALEX_VARIANT);

    public static readonly ImmutableHashSet<FemaleBody> ACCESSORY_COMPATIBLE_FEMALE_BODIES = ImmutableHashSet.Create(
        FemaleBody.CORPORATE, FemaleBody.MECHANIC, FemaleBody.SCIENTIST, FemaleBody.SECURITY, FemaleBody.GLOVELESS);
    public static readonly ImmutableHashSet<MaleBody> ACCESSORY_COMPATIBLE_MALE_BODIES = ImmutableHashSet.Create(MaleBody.CORPORATE,
        MaleBody.DAHL, MaleBody.MECHANIC, MaleBody.SCIENTIST, MaleBody.SECURITY, MaleBody.GLOVELESS);

    public static readonly ImmutableArray<FemaleHead> SUPPORTED_FEMALE_HEADS = ImmutableArray.Create(FemaleHead.HEAD01, FemaleHead.HEAD02,
        FemaleHead.HEAD02_PSYCHOSCOPE, FemaleHead.HEAD03, FemaleHead.HEAD04, FemaleHead.HEAD05, FemaleHead.HEAD06,
        FemaleHead.MIKHAILA, FemaleHead.MORGAN, FemaleHead.SARA, FemaleHead.HELMET);

    public static readonly ImmutableArray<MaleHead> SUPPORTED_MALE_HEADS = ImmutableArray.Create(MaleHead.DAHL, MaleHead.CALVINO, MaleHead.IGWE,
        MaleHead.HEAD01, MaleHead.HEAD01_PSYCHOSCOPE, MaleHead.HEAD01_PSYCHO, MaleHead.HEAD02, MaleHead.HEAD03,
        MaleHead.HEAD03_PSYCHOSCOPE, MaleHead.HEAD04, MaleHead.HEAD05, MaleHead.HEAD06, MaleHead.MORGAN,
        MaleHead.VOLUNTEER01, MaleHead.VOLUNTEER02, MaleHead.VOLUNTEER03, MaleHead.HELMET);

    public static readonly ImmutableArray<LargeMaleHead> SUPPORTED_LARGE_MALE_HEADS = [LargeMaleHead.ALEX, LargeMaleHead.ALEX_VARIANT, LargeMaleHead.LUKA];

    public static readonly ImmutableArray<LargeMaleHair> SUPPORTED_ALEX_HAIRS = [LargeMaleHair.ALEX, LargeMaleHair.ALEX_VARIANT];

    public static readonly ImmutableHashSet<FemaleHead> FEMALE_HEADS_THAT_SHOULD_NOT_HAVE_BARE_HANDS = ImmutableHashSet.Create(FemaleHead.SARA, FemaleHead.HEAD01, FemaleHead.HUSK);
    public static readonly ImmutableHashSet<MaleHead> MALE_HEADS_THAT_SHOULD_NOT_HAVE_BARE_HANDS = ImmutableHashSet.Create(MaleHead.IGWE, MaleHead.HEAD04, MaleHead.HUSK);

    public static readonly ImmutableHashSet<FemaleBody> FEMALE_BODIES_WITH_BARE_HANDS = ImmutableHashSet.Create(FemaleBody.GLOVELESS);
    public static readonly ImmutableHashSet<MaleBody> MALE_BODIES_WITH_BARE_HANDS = ImmutableHashSet.Create(MaleBody.GLOVELESS, MaleBody.VOLUNTEER);

    // Makes sure that "complete" heads do not have hair
    // TODO: Add husk heads?
    public static readonly ImmutableHashSet<FemaleHead> FEMALE_HEADS_THAT_SHOULD_NOT_HAVE_HAIR = ImmutableHashSet.Create(FemaleHead.HEAD02_PSYCHOSCOPE, FemaleHead.HELMET);
    public static readonly ImmutableHashSet<MaleHead> MALE_HEADS_THAT_SHOULD_NOT_HAVE_HAIR = ImmutableHashSet.Create(MaleHead.HEAD01_PSYCHOSCOPE, MaleHead.HEAD01_PSYCHO,
        MaleHead.HEAD01_PSYCHOSCOPE, MaleHead.HELMET, MaleHead.HEAD03_PSYCHOSCOPE, MaleHead.VOLUNTEER04);

    public static readonly string FEMALE_HEAD_HELMET = "objects/accessories/breather/breather3p/breather3p_genfemale01";
    public static readonly string FEMALE_HEAD_HELMET_MTL = "objects/accessories/breather/breather3p/breather1";

    public static readonly string MALE_LABCOAT_LEGS_MODEL = "labcoat/labcoat_genmalelegs01";
    public static readonly string MALE_LABCOAT_HANDS_MODEL = "labcoat/labcoat_genmalehands01";
    public static readonly string MALE_LABCOAT_LIMBS_MTL = "scientist/scientist_genmalebody01";

    public static readonly string MORGAN_FEMALE_GLOVELESS_MTL = "morgankarl/morgan_genfemalebody01_cut_scene";
    public static readonly string MORGAN_MALE_GLOVELESS_MTL = "morgankarl/morgan_genmalebody01_cut_scene";

    // TODO: Bellamy's labcoat seems to be broken, female limbs as well
    public static readonly string MALE_LAB_COAT_LIMBS = "  <Attachment Inheritable=\"0\" Type=\"CA_SKIN\" AName=\"legs\" Binding=\"objects/characters/humans/labcoat/labcoat_genmalelegs01.skin\" Material=\"objects/characters/humans/scientist/scientist_genmalebody01.mtl\" Flags=\"0\"/>\n"
        + "  <Attachment Inheritable=\"0\" Type=\"CA_SKIN\" AName=\"hands\" Binding=\"objects/characters/humans/labcoat/labcoat_genmalehands01.skin\" Material=\"objects/characters/humans/scientist/scientist_genmalebody01.mtl\" Flags=\"0\"/>";
    public static readonly string FEMALE_LAB_COAT_LIMBS = "<Attachment Inheritable=\"0\" Type=\"CA_SKIN\" AName=\"legs_arms\" Binding=\"objects/characters/humans/labcoat/labcoat_genfemalelegsarms01.skin\" Material=\"objects/characters/humans/scientist/scientist_genfemalebody01.mtl\" Flags=\"0\"/>";

    public static readonly string PLUMBER_HAT = "plumber/plumber_genfemalehat01";
    public static readonly string PLUMBER_HAT_MTL = "plumber/plumber_genfemalebody01";

    public static readonly ImmutableArray<string> ACCESSORIES_FEMALE = ["objects/accessories/propulsionpack/propulsionpack3p_genfemale01"];
    public static readonly ImmutableArray<string> ACCESSORIES_FEMALE_MTL = ["objects/accessories/propulsionpack/propulsionpack_3p_01"];
    public static readonly ImmutableArray<string> ACCESSORIES_MALE =
    [
        "objects/characters/humans/dahl/dahl_genmalebackpack01.skin",
        "objects/accessories/propulsionpack/propulsionpack3p_genmale01",
    ];
    public static readonly ImmutableArray<string> ACCESSORIES_MALE_MTL = ["objects/characters/humans/dahl/dahl_genmalebody01.mtl", "objects/accessories/propulsionpack/propulsionpack_3p_01"];
}