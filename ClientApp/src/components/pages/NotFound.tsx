import React from "react";
import { useDocumentTitle } from "../../util/CustomHooks";

const NotFound = () => {
  useDocumentTitle("Page Not Found");

  return (
    <div id="notfound-container">
      <p>Page not found.</p>
    </div>
  );
};

export default NotFound;
