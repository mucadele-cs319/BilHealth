import { useEffect } from "react";

const useDocumentTitle = (title: string) => {
  useEffect(() => {
    document.title = title + " | BilHealth";
  }, [title]);
};

export { useDocumentTitle };
