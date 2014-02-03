Framework "4.0"

properties {
    $project = "CC.TheBench"
    $name = "The Bench"
    $description = "Company Administration"
    $birthYear = 2014
    $maintainers = "David Cumps"
    $company = "Cumps Consulting"
 
    $configuration = "Release"
    $src = resolve-path ".\src"
    $lib = resolve-path ".\lib"
    $out = resolve-path ".\out"
    $build = if ($env:build_number -ne $NULL) { $env:build_number } else { "0" }
    $version = [IO.File]::ReadAllText(".\VERSION.txt") + "." + $build

    $frontend = "CC.TheBench.Frontend.Web"
    $frontendwebui = "$src\$frontend\$frontend.csproj"
    $frontendout = "$out\Web"
    $frontendport = 5001

    $migrations = "CC.TheBench.Frontend.Migrations"
    $migrationsout = "$out\Migrations"
    
    $owinhost = "OwinHost.2.1.0-rtw-30112-762-rel"
}

task default -depends Compile

task Clean -depends CommonAssemblyInfo {
  exec { msbuild /t:clean /v:q /nologo /p:Configuration=$configuration $src\$project.sln }
}

task Compile -depends Clean {
  exec { msbuild /t:build /v:q /nologo /p:Configuration=$configuration $src\$project.sln }
}

task CommonAssemblyInfo {
    $date = Get-Date
    $year = $date.Year
    $copyrightSpan = if ($year -eq $birthYear) { $year } else { "$birthYear-$year" }
    $copyright = "Copyright (c) $copyrightSpan $maintainers"
 
"using System.Reflection;
using System.Runtime.InteropServices;
 
[assembly: ComVisible(false)]
[assembly: AssemblyProduct(""$name"")]
[assembly: AssemblyDescription(""$description"")]
[assembly: AssemblyVersion(""$version"")]
[assembly: AssemblyFileVersion(""$version"")]
[assembly: AssemblyCopyright(""$copyright"")]
[assembly: AssemblyCompany(""$company"")]
[assembly: AssemblyConfiguration(""$configuration"")]" | out-file "$src\CommonAssemblyInfo.cs" -encoding "ASCII"
}

task ServeSite -depends Clean  {
    msbuild /t:rebuild /v:q /nologo /p:OutDir=$frontendout /p:Configuration=$configuration /p:UseWPP_CopyWebApplication=True /p:PipelineDependsOnBuild=False /p:TrackFileAccess=false "$frontendwebui"

    copy $lib\$owinhost\tools\* -destination $frontendout\_PublishedWebsites\$frontend -recurse -force
    
    set-location $frontendout\_PublishedWebsites\$frontend

    start-job -scriptblock {
        param($frontendport)
        start-sleep 5
        start-process "http://localhost:$frontendport" 
    } -arg $frontendport
    
    exec { & "$frontendout\_PublishedWebsites\$frontend\OwinHost.exe" --port $frontendport -o }
}
