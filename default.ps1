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
    $build = if ($env:build_number -ne $NULL) { $env:build_number } else { "0" }
    $version = [IO.File]::ReadAllText(".\VERSION.txt") + "." + $build
}

task default -depends Compile

task Compile -depends CommonAssemblyInfo {
  exec { msbuild /t:clean /v:q /nologo /p:Configuration=$configuration $src\$project.sln }
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