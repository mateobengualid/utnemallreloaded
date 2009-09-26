import "Microsoft", "platform=DotNET", "ns=DotNET", "assembly=mscorlib";
import "System", "platform=DotNET", "ns=DotNET", "assembly=mscorlib";
import "System", "platform=DotNET", "ns=DotNET", "assemblyfilename=%PROGRAMFILES_FOLDER%%NET_FRAMEWORK3_FOLDER%\\System.ServiceModel.dll";
import "UtnEmall", "platform=DotNET", "ns=DotNET", "assemblyfilename=..\\BaseDesktop.dll";
import "UtnEmall", "platform=DotNET", "ns=DotNET", "assemblyfilename=..\\EntityModel.dll";
import "UtnEmall", "platform=DotNET", "ns=DotNET", "assemblyfilename=..\\WPFCore.exe";
import "UtnEmall", "platform=DotNET", "ns=DotNET", "assemblyfilename=%INFRASTRUCTURE_DLL%";


using DotNET::System;
using DotNET::System::IO;
using DotNET::System::Collections;
using DotNET::System::Collections::Generic;
using DotNET::System::Collections::ObjectModel;
using DotNET::System::Collections::Generic;
using DotNET::UtnEmall::Store%STORE_ID%;
using DotNET::UtnEmall::Store%STORE_ID%::EntityModel;
using DotNET::UtnEmall::Server::EntityModel;
using DotNET::UtnEmall::Server::WpfCore;

using UtnEmall::Utils;

namespace %NAMESPACE%{
	
	%SERVICE_CLASS_NAME%::New{
		ServiceName = %SERVICE_NAME%;
		ServiceID = %SERVICE_ID%;
		StoreID = %STORE_ID%;
		ServiceDescription = "%SERVICE_DESCRIPTION%";		
%RELATIONS%		
%DATA_SOURCES%		
%FORMS%
	};
}

