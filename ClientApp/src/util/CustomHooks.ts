import { useEffect } from "react";

export const titleify = (title: string) => {
  return title + " | BilHealth";
};

export const useDocumentTitle = (title: string) => {
  useEffect(() => {
    document.title = titleify(title);
  }, [title]);
};
