// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: catalog load contract tests.
//
// Tests for CatalogLoadResult<T>, CatalogBootValidator, and catalog classification.

using System;
using System.Collections.Generic;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class CatalogLoadContractTests
    {
        [Fact]
        public void CatalogLoadResult_CreatesSuccessfulResult()
        {
            var entries = new List<string> { "a", "b", "c" };
            var result = CatalogLoadResult<string>.Success(
                "/path/to/catalog.json",
                "TestSchema",
                entries,
                schemaVersion: 1,
                classification: CatalogClassification.Required);

            Assert.Equal("/path/to/catalog.json", result.FilePath);
            Assert.Equal("TestSchema", result.Schema);
            Assert.Equal(1, result.SchemaVersion);
            Assert.Equal(CatalogClassification.Required, result.Classification);
            Assert.Equal(3, result.EntryCount);
            Assert.Equal(entries, result.Entries);
            Assert.True(result.IsSuccess);
            Assert.False(result.HasErrors);
            Assert.False(result.HasWarnings);
            Assert.False(result.HasFatalErrors);
        }

        [Fact]
        public void CatalogLoadResult_CreatesFailedResult()
        {
            var ex = new Exception("Test error");
            var result = CatalogLoadResult<string>.Fail(
                "/path/to/catalog.json",
                "TestSchema",
                "File not found",
                ex,
                CatalogClassification.Required);

            Assert.Equal("/path/to/catalog.json", result.FilePath);
            Assert.Equal("TestSchema", result.Schema);
            Assert.Equal(CatalogClassification.Required, result.Classification);
            Assert.Equal(0, result.EntryCount);
            Assert.False(result.IsSuccess);
            Assert.True(result.HasErrors);
            Assert.True(result.HasFatalErrors);
            Assert.Single(result.Messages);
            Assert.Equal(CatalogLoadSeverity.Fatal, result.Messages[0].Severity);
            Assert.Equal("File not found", result.Messages[0].Message);
            Assert.Same(ex, result.Messages[0].Exception);
        }

        [Fact]
        public void CatalogLoadResult_OptionalCatalog_FailIsNotFatal()
        {
            var result = CatalogLoadResult<string>.Fail(
                "/path/to/optional.json",
                "TestSchema",
                "File not found",
                classification: CatalogClassification.Optional);

            Assert.False(result.IsSuccess);
            Assert.True(result.HasErrors);
            Assert.False(result.HasFatalErrors);
            Assert.Equal(CatalogLoadSeverity.Error, result.Messages[0].Severity);
        }

        [Fact]
        public void CatalogLoadResult_ThrowIfFatal_ThrowsOnRequiredFailure()
        {
            var result = CatalogLoadResult<string>.Fail(
                "/path/to/catalog.json",
                "TestSchema",
                "Required catalog failed",
                classification: CatalogClassification.Required);

            var ex = Assert.Throws<InvalidOperationException>(() => result.ThrowIfFatal());
            Assert.Contains("Fatal errors loading required catalog", ex.Message);
            Assert.Contains("Required catalog failed", ex.Message);
        }

        [Fact]
        public void CatalogLoadResult_ThrowIfFatal_NoThrowOnSuccess()
        {
            var result = CatalogLoadResult<string>.Success(
                "/path/to/catalog.json",
                "TestSchema",
                new List<string> { "a" });

            result.ThrowIfFatal(); // Should not throw
        }

        [Fact]
        public void CatalogLoadResult_ThrowIfFatal_NoThrowOnOptionalFailure()
        {
            var result = CatalogLoadResult<string>.Fail(
                "/path/to/optional.json",
                "TestSchema",
                "Optional catalog failed",
                classification: CatalogClassification.Optional);

            result.ThrowIfFatal(); // Should not throw for optional
        }

        [Fact]
        public void CatalogLoadResult_AddMessages()
        {
            var result = new CatalogLoadResult<string>(
                "/path/to/catalog.json",
                "TestSchema",
                CatalogClassification.Required);

            result.AddInfo("Info message");
            result.AddWarning("Warning message");
            result.AddError("Error message");
            result.AddFatal("Fatal message");

            Assert.Equal(4, result.Messages.Count);
            Assert.Equal(CatalogLoadSeverity.Info, result.Messages[0].Severity);
            Assert.Equal(CatalogLoadSeverity.Warning, result.Messages[1].Severity);
            Assert.Equal(CatalogLoadSeverity.Error, result.Messages[2].Severity);
            Assert.Equal(CatalogLoadSeverity.Fatal, result.Messages[3].Severity);
        }

        [Fact]
        public void CatalogLoadResult_FromWrappedListFile_Success()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // This tests the static helper for wrapped list files
            var result = CatalogLoadResult<string>.FromWrappedListFile(
                "/nonexistent/path.json",
                "TestSchema",
                CatalogClassification.Optional,
                json);

            Assert.Equal("/nonexistent/path.json", result.FilePath);
            Assert.Equal("TestSchema", result.Schema);
            Assert.Equal(CatalogClassification.Optional, result.Classification);
            Assert.Equal(0, result.EntryCount);
            // For optional files, missing is not an error
            Assert.Contains("not found (ok)", result.Messages[0].Message);
            Assert.Equal(CatalogLoadSeverity.Info, result.Messages[0].Severity);
        }

        [Fact]
        public void CatalogLoadResult_FromWrappedListFile_RequiredMissing_Throws()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var result = CatalogLoadResult<string>.FromWrappedListFile(
                "/nonexistent/path.json",
                "TestSchema",
                CatalogClassification.Required,
                json);

            Assert.True(result.HasFatalErrors);
            Assert.Single(result.Messages);
            Assert.Equal(CatalogLoadSeverity.Fatal, result.Messages[0].Severity);
            Assert.Contains("Required catalog file not found", result.Messages[0].Message);
        }

        [Fact]
        public void CatalogBootReport_TracksErrors()
        {
            var report = new CatalogBootReport();

            report.AddSuccess("Test1", "test1.json");
            report.AddWarning("Test2", "test2.json", "minor issue");
            report.AddError("Test3", "test3.json", "critical issue");

            Assert.Equal(3, report.Entries.Count);
            Assert.True(report.HasRequiredErrors); // AddError marks isRequired=true
            Assert.True(report.HasErrors);
        }

        [Fact]
        public void CatalogBootReport_TracksRequiredErrors()
        {
            var report = new CatalogBootReport();

            report.AddSuccess("Test1", "test1.json");
            report.AddError("Test2", "test2.json", "required issue");

            // The AddError marks it as required (isRequired=true)
            Assert.True(report.HasRequiredErrors);
            Assert.True(report.HasErrors);
        }

        [Fact]
        public void CatalogBootValidator_ThrowIfRequiredFailed_ThrowsOnRequiredErrors()
        {
            var report = new CatalogBootReport();
            report.AddError("RequiredCatalog", "required.json", "Failed to load");

            var ex = Assert.Throws<InvalidOperationException>(() =>
                CatalogBootValidator.ThrowIfRequiredFailed(report));

            Assert.Contains("Required catalog validation failed", ex.Message);
            Assert.Contains("RequiredCatalog", ex.Message);
        }

        [Fact]
        public void CatalogBootValidator_ThrowIfRequiredFailed_NoThrowOnSuccess()
        {
            var report = new CatalogBootReport();
            report.AddSuccess("Test1", "test1.json");
            report.AddSuccess("Test2", "test2.json");

            CatalogBootValidator.ThrowIfRequiredFailed(report); // Should not throw
        }

        [Fact]
        public void CatalogLoadMessage_ToString_FormatsCorrectly()
        {
            var ex = new Exception("test exception");
            var message = new CatalogLoadMessage(
                CatalogLoadSeverity.Error,
                "/path/to/file.json",
                "TestShape",
                "Test message",
                ex);

            var str = message.ToString();
            Assert.Contains("[Error]", str);
            Assert.Contains("/path/to/file.json", str);
            Assert.Contains("TestShape", str);
            Assert.Contains("Test message", str);
            Assert.Contains("test exception", str);
        }

        [Fact]
        public void CatalogClassification_Values()
        {
            Assert.Equal(0, (int)CatalogClassification.Required);
            Assert.Equal(1, (int)CatalogClassification.Optional);
            Assert.Equal(2, (int)CatalogClassification.DeveloperOnly);
        }

        [Fact]
        public void CatalogLoadSeverity_Values()
        {
            Assert.Equal(0, (int)CatalogLoadSeverity.Info);
            Assert.Equal(1, (int)CatalogLoadSeverity.Warning);
            Assert.Equal(2, (int)CatalogLoadSeverity.Error);
            Assert.Equal(3, (int)CatalogLoadSeverity.Fatal);
        }
    }
}
