{{- /*
Common helper templates for the frenchrevolution chart
*/ -}}

{{- define "frenchrevolution.fullname" -}}
{{- if .Values.fullnameOverride -}}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" -}}
{{- else -}}
{{- .Release.Name | trunc 63 | trimSuffix "-" -}}
{{- end -}}
{{- end }}

{{- define "frenchrevolution.name" -}}
{{- default .Chart.Name .Values.nameOverride -}}
{{- end }}

{{- define "frenchrevolution.labels" -}}
helm.sh/chart: {{ .Chart.Name }}-{{ .Chart.Version | replace "+" "_" }}
app.kubernetes.io/name: {{ include "frenchrevolution.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
app.kubernetes.io/version: {{ .Chart.AppVersion }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{- define "frenchrevolution.selectorLabels" -}}
app.kubernetes.io/name: {{ include "frenchrevolution.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}