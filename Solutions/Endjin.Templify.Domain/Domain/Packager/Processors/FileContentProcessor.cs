namespace Endjin.Templify.Domain.Domain.Packager.Processors
{
    #region Using Directives

    using System.ComponentModel.Composition;
    using System.IO;
    using System.Text;

    using Endjin.Templify.Domain.Contracts.Packager.Processors;

    #endregion

    [Export(typeof(IFileContentProcessor))]
    public class FileContentProcessor : IFileContentProcessor
    {
        public string ReadContents(string path)
        {
            return File.ReadAllText(path);
        }

        public void WriteContents(string path, string content)
        {
            this.MakeWritable(path);

            var hidden = this.IsHidden(path);

            if (hidden)
            {
                this.Unhide(path);
            }

            File.WriteAllText(path, content, Encoding.UTF8);

            if (hidden)
            {
                this.Hide(path);
            }
        }

        private void Hide(string path)
        {
            File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden);
        }

        private void Unhide(string path)
        {
            File.SetAttributes(path, File.GetAttributes(path) & ~FileAttributes.Hidden);
        }

        private bool IsHidden(string path)
        {
            return File.GetAttributes(path).HasFlag(FileAttributes.Hidden);
        }

        private void MakeWritable(string path)
        {
            File.SetAttributes(path, File.GetAttributes(path) & ~(FileAttributes.Archive | FileAttributes.ReadOnly));
        }
    }
}