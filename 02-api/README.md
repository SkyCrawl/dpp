# How to use Ini

## Creating specification

Before creating a configuration, you will need to specify the format of the configuration file. This is very convenient using the popular [Yaml](http://yaml.org) mark-up language. Here is a short example of a *Yaml* specification:
```
sections:
- identifier: Section 1							
  mandatory: true								
  description: the purpose of Section 1			
  options:										
  - !String
    identifier: Option 1						
    mandatory: true								
    description: the purpose of Option 1		
    value: single							
  - !Enum
    identifier: Option 2
    mandatory: false
    description: the purpose of Option 2
    value: multiple
    default_value:								
    - Value1
    allowed_values:
    - Value1
    - Value2
    - Value3
```

See a more detailed example in `02-api-test/Examples/example.yml`.


## Creating configuration

You can easily describe your configuration in a file using key-value pairs for options and their values, grouping them into sections. Here is a small example which adheres to the specification described above:

```
[Section 1]
; the purpose of Section 1 
Option 1 = value 1                  
Option 2  =  Value1:Value2:Value3
```

Again, for a more detailed example, see `02-api-test/Examples/config.txt`.

## Reading configuration

For reading an existing configuration use *ConfigReader*:
```
ConfigReader confRader = new ConfigReader();
```

and for schema *SchemaReader*:
```
SchemaReader specReader = new SchemaReader();
ConfigSpec myConfigSpec = specReader.LoadFromFile(specPath);
```

Configuration can be read in two different modes- either strict, which is the default mode:
```
Config myConfig = reader.ReadFromFile(configPath, configSpec);
```
or relaxed mode:
```
Config myConfig = reader.ReadFromFile(configPath, configSpec, ConfigValidationMode.Relaxed);
```

## Accessing and modifying configuration

To retrieve option's values, specify the section's and option's name. To retrieve the value of *Option 1* from *Section 1* enter:
```
ObservableCollection<StringElement> option1Vals =  myConfig.GetElements("Section 1", "Option 1");
```

To get all options from one section:
```
Section section1 = myConfig.GetSection("Section 1");
```

The configuration can also be modified. For example, a new option can be added:
```
Option option3 = new Option("Option3", bool, "comment for Option 3");
myConfig.Add(option3);
```

## Saving modified configuration

For saving the configuration, use *ConfigWriter*:
```
ConfigWriter writer = new ConfigWriter();
writer.WriteToFile(configPath, myConfig);
```

If you did not explicitly change the order, the sections and options will be written in their original order.

## Creating default configuration

You can also generate a configuration stub containing all those options for which default values have been specified including their commentaries:

```
Config defaultConfig = myConfigSpec.CreateConfigStub();
ConfigWriter writer = new ConfigWriter();
writer.WriteToFile(configPath, deafultConfig);
```
## Loading configuration 

You can also load a configuration into memory using a `TextWriter`:
```
FileStream stream = File.Open(configPath, FileMode.Create, FileAccess.Write);
StreamWriter streamWriter = new StreamWriter(stream, Encoding.Default);
writer.WriteToText(streamWriter, myConfig)
```

That's all. Enjoy!!!
