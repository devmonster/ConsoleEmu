# Console Emu

Emulates a PowerShell console window with scripted text input and output.

Configure a scenario in the `scenario.json` file.

## Sample Full Configuration

```
{
  "Config": {
    "Title": "Windows Powershell",
    "Headers": [ "Microsoft Windows [Version 10.0.19433.563]", "(c) Microsoft Corpration. All rights reserved."  ],
    "Prompt": "PS C:\\Console>",
    "ClearOnStart": true
  },
  "Scenarios": [
    {
      "Input": "az login",
      "OutputStream": {
        "Outputs": [
          {
            "Output": [ "Retrieving tenants and subscriptions for the selection..." ],
            "Delay": 5000
          }      
        ]
      }
    }
  ]
}
```

## Configuration

```
  "Config": {
    "Title": "Console Title",
    "Headers": [ "Header information"  ],
    "Prompt": "PS C:\\Console>",
    "ClearOnStart": true
  },
```

### `Headers[]` string
Text that would appear on the start of the prompt. One line per item

### `Prompt` string
The text prompt value

### `ClearOnStart` bool
Whether the console would clear itself on start

## Scenario configuration

```
"Scenarios" : 
[
  {
    "Input": "sample command",
    "OutputStream": {
      "Outputs": [
        {
          "Output": [ "Sample output text" ],
          "Delay": 5000
        },
        {
          "Output": [
            "",
            "A text after a blank line"
          ],
          "Delay": 10
        },
        {
          "Output": [
            "",
            "A yellow text"
          ],
          "Delay": 5,
          "Color": "Yellow"
        }

      ]
    }
  }
]
```

The `scenario.json` consists of an array of `Input` objects

### `Input` string
Contains the text that will be virtually entered into the console

### `OutputStream` Outputs[]
An array of `Outputs`

### `Outputs` 
`Output[]` Array of text that will be shown as an output, one line per array item  
`Delay` Delay of output in ms
`Color` Console color to be used