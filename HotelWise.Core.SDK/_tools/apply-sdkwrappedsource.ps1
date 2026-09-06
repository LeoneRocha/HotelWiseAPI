# Apply [SdkWrappedSource] to HotelWise.Core.SDK thin wrappers (PR-D6).
# Targets CURRENT post-20260906 FQNs (LongEntityBase, Service.API.Middleware, etc.).
param(
  [string]$Path = "C:\git\HotelWise\HotelWiseAPI\HotelWise.Core.SDK"
)

$ErrorActionPreference = 'Stop'
$sdkRoot = $Path

$overrides = @{
  'EntityBase' = 'SmartCoreHub.Core.SDK.Domain.Entities.Common.LongEntityBase'
  'EntityBaseWithNameEmail' = 'SmartCoreHub.Core.SDK.Domain.Entities.Common.Ported.EntityBaseWithNameEmail'
  'GenericRepositoryBase' = 'SmartCoreHub.Core.SDK.EntityFrameworkCore.Repositories.GenericRepository`2'
  'IGenericRepository' = 'SmartCoreHub.Core.SDK.Infrastructure.Repositories.Generic.IGenericRepository`1'
  'IGenericService' = 'SmartCoreHub.Core.SDK.Domain.Abstractions.IGenericService`1'
  'GenericEntityServiceBase' = 'SmartCoreHub.Core.SDK.Domain.Abstractions.IGenericService`1'
  'CultureDisplayDto' = 'SmartCoreHub.Core.SDK.Domain.DTOs.Entities.CultureDisplayDto'
  'CultureDateTimeHelper' = 'SmartCoreHub.Core.SDK.Domain.Helpers.CultureDateTimeHelper'
  'CorrelationIdMiddleware' = 'SmartCoreHub.Core.SDK.Service.API.Middleware.CorrelationIdMiddleware'
  'RequestLoggingMiddleware' = 'SmartCoreHub.Core.SDK.Service.API.Middleware.RequestLoggingMiddleware'
  'LogAppHelper' = 'SmartCoreHub.Core.SDK.Service.API.Helpers.LogAppHelper'
  'TokenService' = 'SmartCoreHub.Core.SDK.Service.Security.Ported.TokenService'
  'SecurityHelper' = 'SmartCoreHub.Core.SDK.Service.Security.Ported.SecurityHelper'
  'SecurityHelperApi' = 'SmartCoreHub.Core.SDK.Service.Security.Ported.SecurityHelperApi'
  'HelperCharSet' = 'SmartCoreHub.Core.SDK.Infrastructure.Data.Configurations.Helper.Ported.HelperCharSet'
  'GlobalExceptionMiddleware' = 'SmartCoreHub.Core.SDK.Infrastructure.Middleware.GlobalExceptionMiddleware'
  'CoreSdkInfo' = 'SmartCoreHub.Core.SDK.Common.CoreSdkInfo'
  'HelperValidation' = 'SmartCoreHub.Core.SDK.Service.Validation.HelperValidation'
  'ConfigureServicesAI' = 'SmartCoreHub.Core.SDK.Service.AI.Configure.ConfigureServicesAI'
  'SemanticKernelProviderConfigure' = 'SmartCoreHub.Core.SDK.Service.AI.Configure.SemanticKernelProviderConfigure'
  'ChatCompletionValidatorsConstants' = 'SmartCoreHub.Core.SDK.Domain.AI.Constants.ChatCompletionValidatorsConstants'
  'AppConfigConstants' = 'SmartCoreHub.Core.SDK.Common.Constants.AppConfigConstants'
  'AzureADEntraIDConstants' = 'SmartCoreHub.Core.SDK.Common.Constants.AzureADEntraIDConstants'
  'EntityTypeConfigurationConstants' = 'SmartCoreHub.Core.SDK.Common.Constants.EntityTypeConfigurationConstants'
  'ValidatorConstants' = 'SmartCoreHub.Core.SDK.Common.Constants.ValidatorConstants'
  'ChatSessionHelper' = 'SmartCoreHub.Core.SDK.Service.AI.Helpers.ChatSessionHelper'
  'EmbeddingHelper' = 'SmartCoreHub.Core.SDK.Service.AI.Helpers.EmbeddingHelper'
  'TokenCounterHelper' = 'SmartCoreHub.Core.SDK.Service.AI.Helpers.TokenCounterHelper'
  'AIChatServiceType' = 'SmartCoreHub.Core.SDK.Domain.AI.Enums.AIChatServiceType'
  'AIEmbeddingServiceType' = 'SmartCoreHub.Core.SDK.Domain.AI.Enums.AIEmbeddingServiceType'
  'InferenceAiAdapterType' = 'SmartCoreHub.Core.SDK.Domain.AI.Enums.InferenceAiAdapterType'
  'RoleAiPromptsType' = 'SmartCoreHub.Core.SDK.Domain.AI.Enums.RoleAiPromptsType'
  'VectorStoreType' = 'SmartCoreHub.Core.SDK.Domain.AI.Enums.VectorStoreType'
  'ETypeDataBase' = 'SmartCoreHub.Core.SDK.Common.ETypeDataBase'
  'GenericVectorStoreAdapter' = 'SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters.GenericVectorStoreAdapter`1'
  'GroqApiAdapter' = 'SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters.GroqApiAdapter'
  'MistralApiAdapter' = 'SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters.MistralApiAdapter'
  'OllamaAdapter' = 'SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters.OllamaAdapter'
  'SemanticKernelAdapter' = 'SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters.SemanticKernelAdapter'
  'AskAssistantRequestValidator' = 'SmartCoreHub.Core.SDK.Service.AI.Validation.AskAssistantRequestValidator'
  'HistoryPromptsValidator' = 'SmartCoreHub.Core.SDK.Service.AI.Validation.HistoryPromptsValidator'
  'PromptMessageValidator' = 'SmartCoreHub.Core.SDK.Service.AI.Validation.PromptMessageValidator'
  'AIInferenceAdapterFactory' = 'SmartCoreHub.Core.SDK.Service.AI.Services.AIInferenceAdapterFactory'
  'AIInferenceService' = 'SmartCoreHub.Core.SDK.Service.AI.Services.AIInferenceService'
  'VectorStoreAdapterFactory' = 'SmartCoreHub.Core.SDK.Service.AI.Services.VectorStoreAdapterFactory'
  'DataHelper' = 'SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper'
  'HtmlHelper' = 'SmartCoreHub.Core.SDK.Domain.Helpers.HtmlHelper'
  'MarkdownHelper' = 'SmartCoreHub.Core.SDK.Domain.Helpers.MarkdownHelper'
  'TimeFormatter' = 'SmartCoreHub.Core.SDK.Domain.Helpers.TimeFormatter'
  'ConfigurationAppSettingsHelper' = 'SmartCoreHub.Core.SDK.Domain.Helpers.ConfigurationAppSettingsHelper'
  'EnumExtensions' = 'SmartCoreHub.Core.SDK.Domain.Extensions.EnumExtensions'
  'ModelBuilderExtensions' = 'SmartCoreHub.Core.SDK.Infrastructure.Data.ModelBuilderExtensions'
  'ServiceCollectionConfigureCors' = 'SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionConfigureCors'
  'ServiceCollectionConfigureAutoMapper' = 'SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionConfigureAutoMapper'
  'ServiceCollectionHelper' = 'SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionHelper'
  'ServiceCollectionConfigureAppSettings' = 'SmartCoreHub.Core.SDK.Service.DependenciesCollection.Extensions.ServiceCollectionConfigureAppSettings'
  'AskAssistantResponse' = 'SmartCoreHub.Core.SDK.Domain.AI.DTO.AskAssistantResponse'
  'AskAssistantRequest' = 'SmartCoreHub.Core.SDK.Domain.AI.DTO.AskAssistantRequest'
  'DataVectorVO' = 'SmartCoreHub.Core.SDK.Domain.AI.DTO.DataVectorVO'
  'PromptMessageVO' = 'SmartCoreHub.Core.SDK.Domain.AI.DTO.PromptMessageVO'
  'AiInferenceConfigBase' = 'SmartCoreHub.Core.SDK.Domain.AI.Configuration.AiInferenceConfigBase'
  'AzureOpenAIConfig' = 'SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureOpenAIConfig'
  'AzureOpenAIEmbeddingsConfig' = 'SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureOpenAIEmbeddingsConfig'
  'OpenAIConfig' = 'SmartCoreHub.Core.SDK.Domain.AI.Configuration.OpenAIConfig'
  'OpenAIEmbeddingsConfig' = 'SmartCoreHub.Core.SDK.Domain.AI.Configuration.OpenAIEmbeddingsConfig'
  'MistralApiConfig' = 'SmartCoreHub.Core.SDK.Domain.AI.Configuration.MistralApiConfig'
  'MistralApiEmbeddingsConfig' = 'SmartCoreHub.Core.SDK.Domain.AI.Configuration.MistralApiEmbeddingsConfig'
  'GroqApiConfig' = 'SmartCoreHub.Core.SDK.Domain.AI.Configuration.GroqApiConfig'
  'OllamaConfig' = 'SmartCoreHub.Core.SDK.Domain.AI.Configuration.OllamaConfig'
  'AzureAISearchConfig' = 'SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureAISearchConfig'
  'WeaviateConfig' = 'SmartCoreHub.Core.SDK.Domain.AI.Configuration.WeaviateConfig'
  'AzureCosmosDBConfig' = 'SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureCosmosDBConfig'
  'QdrantConfig' = 'SmartCoreHub.Core.SDK.Domain.AI.Configuration.QdrantConfig'
  'RedisConfig' = 'SmartCoreHub.Core.SDK.Domain.AI.Configuration.RedisConfig'
  'AzureAdConfig' = 'SmartCoreHub.Core.SDK.Domain.AI.Configuration.AzureAdConfig'
  'IAIInferenceAdapter' = 'SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceAdapter'
  'IAIInferenceAdapterFactory' = 'SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceAdapterFactory'
  'IAIInferenceService' = 'SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceService'
  'IAssistantService' = 'SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAssistantService'
  'IVectorStoreAdapter' = 'SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreAdapter`1'
  'IVectorStoreAdapterFactory' = 'SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreAdapterFactory'
  'IVectorStoreService' = 'SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IVectorStoreService`1'
}

function Convert-ToAttrTarget([string]$typeFqn) {
  if ([string]::IsNullOrWhiteSpace($typeFqn)) { return $null }
  $t = $typeFqn.Trim().TrimEnd(',')
  if ($t -match '^(.+?)<([^>]+)>$') {
    $base = $Matches[1]
    $argc = ($Matches[2] -split ',').Count
    return ($base + '`' + $argc)
  }
  return $t
}

function Get-InheritSchTarget([string]$inheritClause) {
  if ([string]::IsNullOrWhiteSpace($inheritClause)) { return $null }
  $parts = $inheritClause -split ',' | ForEach-Object { $_.Trim() }
  foreach ($p in $parts) {
    if ($p -match 'SmartCoreHub\.Core\.SDK\.') {
      return Convert-ToAttrTarget $p
    }
  }
  return $null
}

function Get-StaticDelegateTarget([string]$slice) {
  $m = [regex]::Match($slice, 'SmartCoreHub\.Core\.SDK\.[\w\.]+(?=\.\w+\()')
  if ($m.Success) { return $m.Value }
  $m2 = [regex]::Match($slice, '=\s*(SmartCoreHub\.Core\.SDK\.[\w\.]+)\.')
  if ($m2.Success) { return $m2.Groups[1].Value }
  return $null
}

$attrUsing = 'using SmartCoreHub.Core.SDK.Common.Attributes;'
$filesUpdated = 0
$typesAnnotated = 0
$skipped = New-Object System.Collections.Generic.List[string]

$files = Get-ChildItem $sdkRoot -Recurse -Filter *.cs |
  Where-Object { $_.FullName -notmatch '\\_tools\\' }

$typeDeclRegex = [regex]'(?m)^((?:\s*///[^\r\n]*\r?\n|\s*\[[^\]]*\]\r?\n)*)(\s*)((?:public|internal)\s+(?:static\s+)?(?:abstract\s+)?(?:sealed\s+)?(?:partial\s+)?(?:class|interface|struct|enum)\s+)(\w+)(<[^>]+>)?(\s*:\s*([^\{\r\n]+))?'

foreach ($file in $files) {
  $utf8 = [System.Text.UTF8Encoding]::new($false)
  $content = [System.IO.File]::ReadAllText($file.FullName, $utf8)
  if ($content -match '\[SdkWrappedSource') { continue }

  $matches = $typeDeclRegex.Matches($content)
  if ($matches.Count -eq 0) {
    $skipped.Add("$($file.Name): no type decl") | Out-Null
    continue
  }

  $newContent = $content
  for ($i = $matches.Count - 1; $i -ge 0; $i--) {
    $m = $matches[$i]
    $typeName = $m.Groups[4].Value
    $inherit = $m.Groups[7].Value
    $indent = $m.Groups[2].Value
    $pre = $m.Groups[1].Value
    if ($pre -match 'SdkWrappedSource') { continue }

    $target = $null
    if ($overrides.ContainsKey($typeName)) {
      $target = $overrides[$typeName]
    }
    else {
      $target = Get-InheritSchTarget $inherit
      if (-not $target) {
        $start = $m.Index
        $end = if ($i -lt $matches.Count - 1) { $matches[$i + 1].Index } else { $content.Length }
        $slice = $content.Substring($start, $end - $start)
        $target = Get-StaticDelegateTarget $slice
      }
    }

    if (-not $target) {
      $skipped.Add("$($file.Name)::$typeName") | Out-Null
      continue
    }

    $desc = "Casca/wrapper delegando para $target em SmartCoreHub.Core.SDK."
    $attrLine = $indent + '[SdkWrappedSource(targetType: "' + $target + '", targetPackage: "SmartCoreHub.Core.SDK", description: "' + $desc + '")]' + "`r`n"
    $insertAt = $m.Groups[3].Index
    $newContent = $newContent.Insert($insertAt, $attrLine)
    $typesAnnotated++
  }

  if ($newContent -ne $content) {
    if ($newContent -notmatch 'using\s+SmartCoreHub\.Core\.SDK\.Common\.Attributes;') {
      if ($newContent -match '(?m)^namespace\s+') {
        $newContent = [regex]::Replace($newContent, '(?m)^namespace\s+', ($attrUsing + "`r`n`r`nnamespace "), 1)
      }
      elseif ($newContent -match '(?ms)(using\s+[^\r\n]+;\s*)+') {
        $lastUsing = [regex]::Match($newContent, '(?ms)(?:using\s+[^\r\n]+;\s*)+')
        $insertPos = $lastUsing.Index + $lastUsing.Length
        $newContent = $newContent.Insert($insertPos, ($attrUsing + "`r`n"))
      }
      else {
        $newContent = $attrUsing + "`r`n`r`n" + $newContent
      }
    }
    [System.IO.File]::WriteAllText($file.FullName, $newContent, $utf8)
    $filesUpdated++
    Write-Output ("Updated: " + $file.FullName.Substring($sdkRoot.Length + 1))
  }
}

Write-Output ""
Write-Output ("Files updated: " + $filesUpdated)
Write-Output ("Types annotated: " + $typesAnnotated)
Write-Output "Skipped:"
$skipped | Sort-Object -Unique | ForEach-Object { Write-Output ("  " + $_) }

$allCs = Get-ChildItem $sdkRoot -Recurse -Filter *.cs | Where-Object { $_.FullName -notmatch '\\_tools\\' }
$count = (Select-String -Path $allCs.FullName -Pattern '\[SdkWrappedSource' | Measure-Object).Count
Write-Output ("Total [SdkWrappedSource] occurrences: " + $count)
