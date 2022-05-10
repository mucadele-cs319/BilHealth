export const separateCapitalized = (input: string): string => input.split(/(?=[A-Z])/g).join(" ");
