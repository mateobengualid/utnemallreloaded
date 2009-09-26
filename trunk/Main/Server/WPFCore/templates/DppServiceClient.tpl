import "Microsoft", "platform=DotNET", "ns=DotNET", "assembly=mscorlib";
import "System", "platform=DotNET", "ns=DotNET", "assembly=mscorlib";

import "System", "platform=DotNET", "ns=DotNET", "assemblyfilename=%PROGRAMFILES_FOLDER%%COMPACT_FRAMEWORK_FOLDER%\\System.Drawing.dll";
import "System", "platform=DotNET", "ns=DotNET", "assemblyfilename=%PROGRAMFILES_FOLDER%%COMPACT_FRAMEWORK_FOLDER%\\System.dll";
import "System", "platform=DotNET", "ns=DotNET", "assemblyfilename=%PROGRAMFILES_FOLDER%%COMPACT_FRAMEWORK_FOLDER%\\System.Windows.Forms.dll";
import "UtnEmall", "platform=DotNET", "ns=DotNET", "assemblyfilename=libsclient\\BaseMobile.dll";
import "UtnEmall", "platform=DotNET", "ns=DotNET", "assemblyfilename=libsclient\\EntityModel.dll";
import "UtnEmall", "platform=DotNET", "ns=DotNET", "assemblyfilename=libsclient\\BusinessLogic.dll";
import "UtnEmall", "platform=DotNET", "ns=DotNET", "assemblyfilename=libsclient\\DataModel.dll";

using DotNET::System;
using DotNET::System::IO;
using DotNET::System::Collections;
using DotNET::System::Collections::Generic;
using DotNET::System::Collections::ObjectModel;
using DotNET::LayerD::CodeDOM;
using DotNET::LayerD::ZOECompiler;
using DotNET::System::ComponentModel;
using DotNET::System::Data;
using DotNET::System::Drawing;
using DotNET::System::Text;
using DotNET::System::Windows::Forms;
using DotNET::System::Reflection;
using Zoe;
using zoe::lang;
using DotNET::UtnEmall::Client::SmartClientLayer;

using DotNET::System::ServiceModel;
using DotNET::System::ServiceModel::Channels;
using DotNET::System::Runtime::Serialization;
using Microsoft::Tools::ServiceModel;
import "System", "platform=DotNET", "ns=DotNET", "assemblyfilename=%PROGRAMFILES_FOLDER%%COMPACT_FRAMEWORK_FOLDER%\\System.Xml.dll";
import "System", "platform=DotNET", "ns=DotNET", "assemblyfilename=%PROGRAMFILES_FOLDER%%COMPACT_FRAMEWORK_FOLDER%\\System.ServiceModel.dll";
import "System", "platform=DotNET", "ns=DotNET", "assemblyfilename=%PROGRAMFILES_FOLDER%%COMPACT_FRAMEWORK_FOLDER%\\System.Runtime.Serialization.dll";

import "UtnEmall", "platform=DotNET", "ns=DotNET", "assemblyfilename=%INFRASTRUCTURE_DLL%";
using DotNET::UtnEmall::Store%STORE_ID%;
using DotNET::UtnEmall::Store%STORE_ID%::EntityModel;
using DotNET::UtnEmall::Store%STORE_ID%::Services;

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

