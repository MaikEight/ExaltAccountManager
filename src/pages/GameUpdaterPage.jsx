
import { tauri } from '@tauri-apps/api';
import { getAppInit, getGameFileList } from '../backend/decaApi';
import StyledButton from './../components/StyledButton';
import useUserSettings from '../hooks/useUserSettings';
import { LinearProgress } from '@mui/material';
import { useState } from 'react';

function GameUpdaterPage() {
    const settings = useUserSettings();
    const [timeInMs, setTimeInMs] = useState(-1);

    const testFiles = [
        {
            "file": "baselib.dll",
            "checksum": "390513b8afa2c61a1b45de49104ffb29",
            "permision": "",
            "size": 206273
        },
        {
            "file": "GameAssembly.dll",
            "checksum": "4e0907b83ab959b0ec7de04f754f646d",
            "permision": "",
            "size": 22584041
        },
        {
            "file": "RotMG Exalt.exe",
            "checksum": "2cf8ed2e7373cdb45c24bc3af2289191",
            "permision": "",
            "size": 319068
        },
        {
            "file": "UnityCrashHandler64.exe",
            "checksum": "71ab699b91469e2a87419dfd4f349bbd",
            "permision": "",
            "size": 416410
        },
        {
            "file": "UnityPlayer.dll",
            "checksum": "a9f2cc0949a3797800b7dc82809ec846",
            "permision": "",
            "size": 13362628
        },
        {
            "file": "RotMG Exalt_Data/app.info",
            "checksum": "d97d8bc24038dd25567e0d45342b72f5",
            "permision": "",
            "size": 56
        },
        {
            "file": "RotMG Exalt_Data/boot.config",
            "checksum": "c0fb754e840849e463b204690a3a17d2",
            "permision": "",
            "size": 117
        },
        {
            "file": "RotMG Exalt_Data/globalgamemanagers",
            "checksum": "799a016cb37336ed0654a0ea88ed6090",
            "permision": "",
            "size": 1028593
        },
        {
            "file": "RotMG Exalt_Data/globalgamemanagers.assets",
            "checksum": "43070f98e03d127c8a698500c9ca06ec",
            "permision": "",
            "size": 90787
        },
        {
            "file": "RotMG Exalt_Data/globalgamemanagers.assets.resS",
            "checksum": "881f69325b9266f35b00cee05005f427",
            "permision": "",
            "size": 1670777
        },
        {
            "file": "RotMG Exalt_Data/level0",
            "checksum": "fa7c8d9615efa498e8fd43e4fefc7c9e",
            "permision": "",
            "size": 1653
        },
        {
            "file": "RotMG Exalt_Data/level1",
            "checksum": "f1a75f72313185e10dce326fa1cb6b84",
            "permision": "",
            "size": 133160
        },
        {
            "file": "RotMG Exalt_Data/level2",
            "checksum": "ccc0e8a088a3169a9626b25abc4c30f3",
            "permision": "",
            "size": 3450
        },
        {
            "file": "RotMG Exalt_Data/level3",
            "checksum": "ce02ce2c90f66f0db5eac2b7fcab645d",
            "permision": "",
            "size": 10814
        },
        {
            "file": "RotMG Exalt_Data/level4",
            "checksum": "537606cad1846629b942b208a10fd55a",
            "permision": "",
            "size": 207483
        },
        {
            "file": "RotMG Exalt_Data/level5",
            "checksum": "8108a260665083393ade303ab97975b3",
            "permision": "",
            "size": 2072
        },
        {
            "file": "RotMG Exalt_Data/resources.assets",
            "checksum": "ef43b9af87455cb1e7e6f0cdff406614",
            "permision": "",
            "size": 25132664
        },
        {
            "file": "RotMG Exalt_Data/resources.assets.resS",
            "checksum": "037b1bef73511fb060571b9c78dc335d",
            "permision": "",
            "size": 21586498
        },
        {
            "file": "RotMG Exalt_Data/resources.resource",
            "checksum": "ef79fa9baa2341e13c6b78bb211c2090",
            "permision": "",
            "size": 172964656
        },
        {
            "file": "RotMG Exalt_Data/RuntimeInitializeOnLoads.json",
            "checksum": "82b2589ac24a43de9e9e9262957bb559",
            "permision": "",
            "size": 765
        },
        {
            "file": "RotMG Exalt_Data/ScriptingAssemblies.json",
            "checksum": "df2dbe58db0a472ab79ed7d31d46f0c2",
            "permision": "",
            "size": 1011
        },
        {
            "file": "RotMG Exalt_Data/sharedassets0.assets",
            "checksum": "f1c71fb112fb7831ffef1fcd6cad0ee9",
            "permision": "",
            "size": 4236
        },
        {
            "file": "RotMG Exalt_Data/sharedassets0.assets.resS",
            "checksum": "982b904099aa3b52e324c37f6876c432",
            "permision": "",
            "size": 4007914
        },
        {
            "file": "RotMG Exalt_Data/sharedassets1.assets",
            "checksum": "8cb84f9c99317d313c197e2aecca430d",
            "permision": "",
            "size": 257333
        },
        {
            "file": "RotMG Exalt_Data/sharedassets1.assets.resS",
            "checksum": "f2b0d27cf68ae1a7e32d8bc15cb025d1",
            "permision": "",
            "size": 1330454
        },
        {
            "file": "RotMG Exalt_Data/sharedassets2.assets",
            "checksum": "ff192967a203914525668ec578981d17",
            "permision": "",
            "size": 1604
        },
        {
            "file": "RotMG Exalt_Data/sharedassets3.assets",
            "checksum": "81b5ac35c366567c84cacd38e6413c0f",
            "permision": "",
            "size": 27293
        },
        {
            "file": "RotMG Exalt_Data/sharedassets3.assets.resS",
            "checksum": "ef2b906fb713cb31278349adb0809855",
            "permision": "",
            "size": 4249927
        },
        {
            "file": "RotMG Exalt_Data/sharedassets4.assets",
            "checksum": "efb84c7444e49a0185df568925aab54a",
            "permision": "",
            "size": 45254
        },
        {
            "file": "RotMG Exalt_Data/sharedassets4.assets.resS",
            "checksum": "e8465e0caea7e11d8a1b7bd2ec944df7",
            "permision": "",
            "size": 3823
        },
        {
            "file": "RotMG Exalt_Data/sharedassets5.assets",
            "checksum": "09ae21bdefe3f88d64a825eba697831b",
            "permision": "",
            "size": 230
        },
        {
            "file": "RotMG Exalt_Data/version.txt",
            "checksum": "3c7fc01dc7ad30058eeb2e81a520a3d6",
            "permision": "",
            "size": 32
        },
        {
            "file": "RotMG Exalt_Data/Plugins/cef.pak",
            "checksum": "c2c9a44e8b4a9d3f2db354dd75da3489",
            "permision": "",
            "size": 2151607
        },
        {
            "file": "RotMG Exalt_Data/Plugins/cef_100_percent.pak",
            "checksum": "277a53a3922d71cc99626ab835cc8677",
            "permision": "",
            "size": 253373
        },
        {
            "file": "RotMG Exalt_Data/Plugins/cef_200_percent.pak",
            "checksum": "1822748dcb06d101954426fe75a62eb3",
            "permision": "",
            "size": 379000
        },
        {
            "file": "RotMG Exalt_Data/Plugins/cef_extensions.pak",
            "checksum": "597e878419411cc2ce35029b9c44cb72",
            "permision": "",
            "size": 1029736
        },
        {
            "file": "RotMG Exalt_Data/Plugins/chrome_elf.dll",
            "checksum": "502929a7065a77894b6754d8787cf1e4",
            "permision": "",
            "size": 361145
        },
        {
            "file": "RotMG Exalt_Data/Plugins/d3dcompiler_47.dll",
            "checksum": "c9d9786ebbd0639de1dd3e07dab09c60",
            "permision": "",
            "size": 1875637
        },
        {
            "file": "RotMG Exalt_Data/Plugins/devtools_resources.pak",
            "checksum": "766eba8610853eb8c8985a3bbe44f6f3",
            "permision": "",
            "size": 1434399
        },
        {
            "file": "RotMG Exalt_Data/Plugins/icudtl.dat",
            "checksum": "9705ff0fa594bb28520963db19f5471f",
            "permision": "",
            "size": 4527803
        },
        {
            "file": "RotMG Exalt_Data/Plugins/libEGL.dll",
            "checksum": "fb5fec3cbfc38dfcc145d5c508003c9f",
            "permision": "",
            "size": 73987
        },
        {
            "file": "RotMG Exalt_Data/Plugins/libGLESv2.dll",
            "checksum": "0264c47cda7957720f84831738809c81",
            "permision": "",
            "size": 2088348
        },
        {
            "file": "RotMG Exalt_Data/Plugins/natives_blob.bin",
            "checksum": "d2414b8ae71f3f827b984167054e21a1",
            "permision": "",
            "size": 12973
        },
        {
            "file": "RotMG Exalt_Data/Plugins/snapshot_blob.bin",
            "checksum": "ea4ad096fd3526228d2563ec81a886e4",
            "permision": "",
            "size": 68524
        },
        {
            "file": "RotMG Exalt_Data/Plugins/ThirdPartyNotices",
            "checksum": "2e4a9042440ca31c7e96bc420bbf2a31",
            "permision": "",
            "size": 199641
        },
        {
            "file": "RotMG Exalt_Data/Plugins/v8_context_snapshot.bin",
            "checksum": "ed3e6287b042714b8043efcb301d42ca",
            "permision": "",
            "size": 194871
        },
        {
            "file": "RotMG Exalt_Data/Plugins/ZFGameBrowser.exe",
            "checksum": "847d34cc0c281bd0b322aa1d1721a24b",
            "permision": "",
            "size": 429388
        },
        {
            "file": "RotMG Exalt_Data/Plugins/ZFProxyWeb.dll",
            "checksum": "fd964e83e06bcb34fa2d10a889c59f4f",
            "permision": "",
            "size": 263783
        },
        {
            "file": "RotMG Exalt_Data/Plugins/zf_cef.dll",
            "checksum": "ffd14fcb2eb6ddd34d6ddebbc552299b",
            "permision": "",
            "size": 48743495
        },
        {
            "file": "RotMG Exalt_Data/Resources/browser_assets",
            "checksum": "6dbc62ca7bf8372c770e1211817b3614",
            "permision": "",
            "size": 32
        },
        {
            "file": "RotMG Exalt_Data/Resources/unity default resources",
            "checksum": "ad4278530791cff0c07736b9af43e173",
            "permision": "",
            "size": 315778
        },
        {
            "file": "RotMG Exalt_Data/Resources/unity_builtin_extra",
            "checksum": "6e5eacd3013179390dd295dccc15b6b7",
            "permision": "",
            "size": 62808
        },
        {
            "file": "RotMG Exalt_Data/StreamingAssets/UnityServicesProjectConfiguration.json",
            "checksum": "ec1b6e881dc9337f07c950fa13ff368e",
            "permision": "",
            "size": 391
        },
        {
            "file": "RotMG Exalt_Data/il2cpp_data/Metadata/global-metadata.dat",
            "checksum": "120a4743ff360f13bd4722f09449c385",
            "permision": "",
            "size": 4336205
        },
        {
            "file": "RotMG Exalt_Data/il2cpp_data/Resources/mscorlib.dll-resources.dat",
            "checksum": "21d06dbc8af6432b2b49536ed30609af",
            "permision": "",
            "size": 127411
        },
        {
            "file": "RotMG Exalt_Data/il2cpp_data/Resources/System.Data.dll-resources.dat",
            "checksum": "4860ddd4350579f8fcacb1881582335a",
            "permision": "",
            "size": 7946
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/am.pak",
            "checksum": "3e292ba6a915ec8b8f2408e71c03425f",
            "permision": "",
            "size": 85798
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/ar.pak",
            "checksum": "7f1598c653aaa2879ecc1cc20cd9f515",
            "permision": "",
            "size": 83636
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/bg.pak",
            "checksum": "60c21da051795a1cf427d51ff7b8084f",
            "permision": "",
            "size": 87443
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/bn.pak",
            "checksum": "f4018fb82d1baeed446f6da78836c2db",
            "permision": "",
            "size": 93138
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/ca.pak",
            "checksum": "77f47ce82e5bfc2200c7abb88b267123",
            "permision": "",
            "size": 74458
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/cs.pak",
            "checksum": "469e61806b2ab8ffb9155a03896c8302",
            "permision": "",
            "size": 79471
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/da.pak",
            "checksum": "6c623ae0860ccc782fc004667c78ee78",
            "permision": "",
            "size": 71471
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/de.pak",
            "checksum": "0e3b31f511b316bfd87c92c5550f1cb7",
            "permision": "",
            "size": 75355
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/el.pak",
            "checksum": "7d54147bd7ce0e3289cd2c68ef3395a9",
            "permision": "",
            "size": 94165
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/en-GB.pak",
            "checksum": "e06bee2c069738c1761b1f07a5a65c3e",
            "permision": "",
            "size": 67055
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/en-US.pak",
            "checksum": "dc99f78630d32819ebce696dafd26579",
            "permision": "",
            "size": 67217
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/es-419.pak",
            "checksum": "3b0dbd76f824d646f684a1eb7782bb60",
            "permision": "",
            "size": 74162
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/es.pak",
            "checksum": "d546d7722a37ba055ab8a33b682b8a19",
            "permision": "",
            "size": 73723
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/et.pak",
            "checksum": "468578bb57b0616b1fef43167cec075c",
            "permision": "",
            "size": 72709
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/fa.pak",
            "checksum": "74cb66f8ed8c4e708a687f8b6e018abc",
            "permision": "",
            "size": 82212
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/fi.pak",
            "checksum": "a86418f62294a4db2d45b1dd662300ab",
            "permision": "",
            "size": 73095
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/fil.pak",
            "checksum": "e9e9b5738dcc5c64c0564ebee8cb3f48",
            "permision": "",
            "size": 74320
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/fr.pak",
            "checksum": "2352a2bb3481eea03c76a67b8677918e",
            "permision": "",
            "size": 77245
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/gu.pak",
            "checksum": "79886060308ca116d2e8916e1682b828",
            "permision": "",
            "size": 90988
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/he.pak",
            "checksum": "64e7fdcd09abd5e14521f6e49fd2436a",
            "permision": "",
            "size": 77768
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/hi.pak",
            "checksum": "e9a7bc6dfa4016bda1a4faadd0a96086",
            "permision": "",
            "size": 92621
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/hr.pak",
            "checksum": "dbb5a619e31e3958b7ee5df2e68a44da",
            "permision": "",
            "size": 75825
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/hu.pak",
            "checksum": "83f2bff36db68815963734c6f14d2cea",
            "permision": "",
            "size": 79442
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/id.pak",
            "checksum": "eff880916f5f7ca92fef688128e7b2f2",
            "permision": "",
            "size": 68127
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/it.pak",
            "checksum": "b1af2ee608bce8b124401f0f1a8e1719",
            "permision": "",
            "size": 72609
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/ja.pak",
            "checksum": "fb94a46c081766f72924c0234fcb53da",
            "permision": "",
            "size": 75203
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/kn.pak",
            "checksum": "a125f1889817658c5c2687ee8f9f321d",
            "permision": "",
            "size": 94284
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/ko.pak",
            "checksum": "a25bcd561eb7da40b12e4670d751d8e4",
            "permision": "",
            "size": 70958
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/lt.pak",
            "checksum": "c2245554244523776eca368a82983ee6",
            "permision": "",
            "size": 77751
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/lv.pak",
            "checksum": "a95048f767306a131b876df74cf55348",
            "permision": "",
            "size": 77473
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/ml.pak",
            "checksum": "3ebdfd0ab9999b5ad515eb052dfdc577",
            "permision": "",
            "size": 99125
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/mr.pak",
            "checksum": "f242e8272db35e1174b498bbb0aa4949",
            "permision": "",
            "size": 91227
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/ms.pak",
            "checksum": "c8a712e9cbd091697903a118f8e4afe3",
            "permision": "",
            "size": 68535
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/nb.pak",
            "checksum": "d25fafbf9671d948c510f4fedd257703",
            "permision": "",
            "size": 70898
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/nl.pak",
            "checksum": "34bb7a19b695fc9cd97e3bfe644a6bb0",
            "permision": "",
            "size": 72573
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/pl.pak",
            "checksum": "92a70e7431a2d76f54070f86f99bcdfd",
            "permision": "",
            "size": 77925
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/pt-BR.pak",
            "checksum": "f2d645cd25559f8ade54d1372f864294",
            "permision": "",
            "size": 73414
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/pt-PT.pak",
            "checksum": "8b0bd3c98add6936eb5739c025a87c5f",
            "permision": "",
            "size": 73109
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/ro.pak",
            "checksum": "657288dc93e6afeeec4e561ef672cff8",
            "permision": "",
            "size": 75917
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/ru.pak",
            "checksum": "5ef23ee12e24e62ae4fe9e722b689259",
            "permision": "",
            "size": 87778
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/sk.pak",
            "checksum": "4ccd61ea569cd6f9b8eaa0369c2ce65a",
            "permision": "",
            "size": 80428
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/sl.pak",
            "checksum": "e63debb39b36c53f318bed0f2596c47b",
            "permision": "",
            "size": 75460
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/sr.pak",
            "checksum": "7dd73adf36b7079aa327a6bd1ee7b6d1",
            "permision": "",
            "size": 85992
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/sv.pak",
            "checksum": "c4f1eb7fb44af1b85a13e879bf61cd10",
            "permision": "",
            "size": 70963
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/sw.pak",
            "checksum": "26e9cf4b3b1f71dae9fb80a9385788fd",
            "permision": "",
            "size": 71640
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/ta.pak",
            "checksum": "432e8bbc51ddd478ab5d62299bf2467f",
            "permision": "",
            "size": 92964
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/te.pak",
            "checksum": "ed0bb1f4e9cb2b0782f06efa2221cdb1",
            "permision": "",
            "size": 93733
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/th.pak",
            "checksum": "e329d78b773cb1cd5656738ab2d42a10",
            "permision": "",
            "size": 82520
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/tr.pak",
            "checksum": "7001302d34d2d8aeb43c8b380c74f089",
            "permision": "",
            "size": 73477
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/uk.pak",
            "checksum": "a2b8811eba5f639f1b3c92680ee0809e",
            "permision": "",
            "size": 87557
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/vi.pak",
            "checksum": "a826b1fdca25aa1392e1adbba22fec72",
            "permision": "",
            "size": 72851
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/zh-CN.pak",
            "checksum": "07d6a14b26c86d6023d32cd877fac744",
            "permision": "",
            "size": 71093
        },
        {
            "file": "RotMG Exalt_Data/Plugins/locales/zh-TW.pak",
            "checksum": "16afe92b4a89caa7b72cdfa2c6f84a1c",
            "permision": "",
            "size": 70469
        },
        {
            "file": "RotMG Exalt_Data/Plugins/x86_64/lib_burst_generated.dll",
            "checksum": "665c378d2e92de1e8b8560af6f8369f4",
            "permision": "",
            "size": 33851
        },
        {
            "file": "RotMG Exalt_Data/Plugins/x86_64/steam_api64.dll",
            "checksum": "c42cb80f2e1986b5364e0b9f1d454176",
            "permision": "",
            "size": 137221
        }
    ];

    const getClientBuildHash = () => {
        getAppInit()
            .then((appInit) => {
                const appSettings = appInit.AppSettings;
                getFileList(appSettings.BuildHash);
            });
    };

    const getFileList = (buildHash) => {
        console.log("buildHash: ", buildHash);
        getGameFileList(buildHash)
            .then((fileList) => {
                console.log('fileList: ', fileList);
                tauri.invoke({
                    cmd: 'get_game_files_to_update',
                    game_exe_path: settings.getByKeyAndSubKey('game', 'exePath'),
                    game_files_data: JSON.stringify(fileList),
                });
            });
    };

    return (
        <div>
            <h1>Game Updater Page</h1>
            {(timeInMs < 0) && <LinearProgress />}
            <br />
            <StyledButton
                onClick={() => {
                    console.log("search for updates...");
                    getClientBuildHash();
                }}
            >
                search for updates
            </StyledButton>

            <StyledButton
                sx={{ marginLeft: '10px' }}
                onClick={() => {
                    setTimeInMs(-1);
                    const fileList = JSON.stringify(testFiles);
                    console.time("get_game_files_to_update");
                    const startTime = performance.now();
                    tauri.invoke(
                        'get_game_files_to_update',
                        {
                            args: {
                                game_exe_path: settings.getByKeyAndSubKey('game', 'exePath'),
                                game_files_data: fileList,
                            }
                        }).then((result) => {
                            console.log('result: ', result);
                            const endTime = performance.now();
                            const timeNeeded = Math.floor(endTime - startTime);
                            console.log("time needed: ", timeNeeded);
                            setTimeInMs(timeNeeded);
                        }).catch((error) => {
                            console.error('error: ', error);
                            const endTime = performance.now();
                            const timeNeeded = Math.floor(endTime - startTime);
                            console.log("time needed: ", timeNeeded);
                            setTimeInMs(timeNeeded);
                        });
                }}
            >
                TEST
            </StyledButton>
            {
                (timeInMs > 0) &&
                <div style={{display: 'block', marginTop: 8}}>Time needed: {timeInMs} ms</div>
            }
        </div>
    );
}

export default GameUpdaterPage;