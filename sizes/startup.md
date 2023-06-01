# Startup times

Just recording these over time. A `Release` build on a Pixel 5 device.

Spice 🌶:
```log
06-01 14:43:20.843  2084  2346 I ActivityTaskManager: Displayed com.companyname.HeadToHeadSpice/crc6421a68941fd0c4613.MainActivity: +247ms
06-01 14:43:22.248  2084  2346 I ActivityTaskManager: Displayed com.companyname.HeadToHeadSpice/crc6421a68941fd0c4613.MainActivity: +249ms
06-01 14:43:23.641  2084  2346 I ActivityTaskManager: Displayed com.companyname.HeadToHeadSpice/crc6421a68941fd0c4613.MainActivity: +237ms
06-01 14:43:25.040  2084  2346 I ActivityTaskManager: Displayed com.companyname.HeadToHeadSpice/crc6421a68941fd0c4613.MainActivity: +255ms
06-01 14:43:26.453  2084  2346 I ActivityTaskManager: Displayed com.companyname.HeadToHeadSpice/crc6421a68941fd0c4613.MainActivity: +247ms
06-01 14:43:27.848  2084  2346 I ActivityTaskManager: Displayed com.companyname.HeadToHeadSpice/crc6421a68941fd0c4613.MainActivity: +239ms
06-01 14:43:29.267  2084  2346 I ActivityTaskManager: Displayed com.companyname.HeadToHeadSpice/crc6421a68941fd0c4613.MainActivity: +247ms
06-01 14:43:30.654  2084  2346 I ActivityTaskManager: Displayed com.companyname.HeadToHeadSpice/crc6421a68941fd0c4613.MainActivity: +231ms
06-01 14:43:32.073  2084  2346 I ActivityTaskManager: Displayed com.companyname.HeadToHeadSpice/crc6421a68941fd0c4613.MainActivity: +248ms
06-01 14:43:33.497  2084  2346 I ActivityTaskManager: Displayed com.companyname.HeadToHeadSpice/crc6421a68941fd0c4613.MainActivity: +251ms
Average(ms): 245.1
Std Err(ms): 2.28254244210266
Std Dev(ms): 7.21803297304743

Size: 7018603 com.companyname.HeadToHeadSpice-Signed.apk
```

.NET MAUI:
```log
06-01 14:50:15.070  2084  2346 I ActivityTaskManager: Displayed com.companyname.headtoheadmaui/crc649f845fb8d5de61df.MainActivity: +543ms
06-01 14:50:16.763  2084  2346 I ActivityTaskManager: Displayed com.companyname.headtoheadmaui/crc649f845fb8d5de61df.MainActivity: +539ms
06-01 14:50:18.466  2084  2346 I ActivityTaskManager: Displayed com.companyname.headtoheadmaui/crc649f845fb8d5de61df.MainActivity: +547ms
06-01 14:50:20.183  2084  2346 I ActivityTaskManager: Displayed com.companyname.headtoheadmaui/crc649f845fb8d5de61df.MainActivity: +562ms
06-01 14:50:21.872  2084  2346 I ActivityTaskManager: Displayed com.companyname.headtoheadmaui/crc649f845fb8d5de61df.MainActivity: +539ms
06-01 14:50:23.559  2084  2346 I ActivityTaskManager: Displayed com.companyname.headtoheadmaui/crc649f845fb8d5de61df.MainActivity: +534ms
06-01 14:50:25.238  2084  2346 I ActivityTaskManager: Displayed com.companyname.headtoheadmaui/crc649f845fb8d5de61df.MainActivity: +543ms
06-01 14:50:26.934  2084  2346 I ActivityTaskManager: Displayed com.companyname.headtoheadmaui/crc649f845fb8d5de61df.MainActivity: +542ms
06-01 14:50:28.659  2084  2346 I ActivityTaskManager: Displayed com.companyname.headtoheadmaui/crc649f845fb8d5de61df.MainActivity: +562ms
06-01 14:50:30.361  2084  2346 I ActivityTaskManager: Displayed com.companyname.headtoheadmaui/crc649f845fb8d5de61df.MainActivity: +542ms
Average(ms): 545.3
Std Err(ms): 2.98161030317511
Std Dev(ms): 9.42867965305853

Size: 12255930 com.companyname.headtoheadmaui-Signed.apk
```

`dotnet new spice-blazor`:
```log
06-01 15:28:30.423  2084  2346 I ActivityTaskManager: Displayed com.companyname.spice.blazor/crc64ea1cd60bbe11594e.MainActivity: +396ms
06-01 15:28:31.995  2084  2346 I ActivityTaskManager: Displayed com.companyname.spice.blazor/crc64ea1cd60bbe11594e.MainActivity: +414ms
06-01 15:28:33.580  2084  2346 I ActivityTaskManager: Displayed com.companyname.spice.blazor/crc64ea1cd60bbe11594e.MainActivity: +417ms
06-01 15:28:35.170  2084  2346 I ActivityTaskManager: Displayed com.companyname.spice.blazor/crc64ea1cd60bbe11594e.MainActivity: +431ms
06-01 15:28:36.771  2084  2346 I ActivityTaskManager: Displayed com.companyname.spice.blazor/crc64ea1cd60bbe11594e.MainActivity: +426ms
06-01 15:28:38.362  2084  2346 I ActivityTaskManager: Displayed com.companyname.spice.blazor/crc64ea1cd60bbe11594e.MainActivity: +408ms
06-01 15:28:39.930  2084  2346 I ActivityTaskManager: Displayed com.companyname.spice.blazor/crc64ea1cd60bbe11594e.MainActivity: +420ms
06-01 15:28:41.509  2084  2346 I ActivityTaskManager: Displayed com.companyname.spice.blazor/crc64ea1cd60bbe11594e.MainActivity: +432ms
06-01 15:28:43.059  2084  2346 I ActivityTaskManager: Displayed com.companyname.spice.blazor/crc64ea1cd60bbe11594e.MainActivity: +394ms
06-01 15:28:44.615  2084  2346 I ActivityTaskManager: Displayed com.companyname.spice.blazor/crc64ea1cd60bbe11594e.MainActivity: +412ms
Average(ms): 415
Std Err(ms): 4.15799096787005
Std Dev(ms): 13.1487219488773
```

`dotnet new maui-blazor`:
```log
06-01 15:51:14.648  2084  2346 I ActivityTaskManager: Displayed com.companyname.foo/crc64808a40cc7e533249.MainActivity: +653ms
06-01 15:51:16.511  2084  2346 I ActivityTaskManager: Displayed com.companyname.foo/crc64808a40cc7e533249.MainActivity: +641ms
06-01 15:51:18.327  2084  2346 I ActivityTaskManager: Displayed com.companyname.foo/crc64808a40cc7e533249.MainActivity: +640ms
06-01 15:51:20.086  2084  2346 I ActivityTaskManager: Displayed com.companyname.foo/crc64808a40cc7e533249.MainActivity: +628ms
06-01 15:51:21.885  2084  2346 I ActivityTaskManager: Displayed com.companyname.foo/crc64808a40cc7e533249.MainActivity: +631ms
06-01 15:51:23.650  2084  2346 I ActivityTaskManager: Displayed com.companyname.foo/crc64808a40cc7e533249.MainActivity: +612ms
06-01 15:51:25.438  2084  2346 I ActivityTaskManager: Displayed com.companyname.foo/crc64808a40cc7e533249.MainActivity: +635ms
06-01 15:51:27.243  2084  2346 I ActivityTaskManager: Displayed com.companyname.foo/crc64808a40cc7e533249.MainActivity: +643ms
06-01 15:51:29.037  2084  2346 I ActivityTaskManager: Displayed com.companyname.foo/crc64808a40cc7e533249.MainActivity: +633ms
06-01 15:51:30.847  2084  2346 I ActivityTaskManager: Displayed com.companyname.foo/crc64808a40cc7e533249.MainActivity: +650ms
Average(ms): 636.6
Std Err(ms): 3.7214095298541
Std Dev(ms): 11.7681302205953
```
