window.downloadFileFromStream = async (fileName, contentStreamReference) => {
  const arrayBuffer = await contentStreamReference.arrayBuffer();
  const blob = new Blob([arrayBuffer], { type: 'application/octet-stream' });
  const url = URL.createObjectURL(blob);
  const anchorElement = document.createElement('a');
  anchorElement.href = url;
  anchorElement.download = fileName.substring(fileName.lastIndexOf('/') + 1);
  anchorElement.click();
  anchorElement.remove();
  URL.revokeObjectURL(url);
}

window.openFileInBrowser = async (fileName, contentStreamReference) => {
  const mimeTypes = {
    '.pdf':  'application/pdf',
  };
  const ext = fileName.substring(fileName.lastIndexOf('.')).toLowerCase();
  const mimeType = mimeTypes[ext] ?? 'application/octet-stream';
  const arrayBuffer = await contentStreamReference.arrayBuffer();
  const blob = new Blob([arrayBuffer], { type: mimeType });
  const url = URL.createObjectURL(blob);
  const anchor = document.createElement('a');
  anchor.href = url;

  if (ext === '.pdf') {
    // Browsers have a built-in PDF viewer — open in a new tab without a download hint.
    // Do NOT revoke the URL immediately; the new tab needs it while the file is open.
    anchor.target = '_blank';
    anchor.click();
    anchor.remove();
  } else {
    // Browsers cannot render Office documents natively. A blob URL opened via
    // window.open has no filename hint so the browser saves it with a GUID name.
    // Use the download anchor instead so the correct filename is preserved.
    anchor.download = fileName.substring(fileName.lastIndexOf('/') + 1);
    anchor.click();
    anchor.remove();
    URL.revokeObjectURL(url);
  }
}
