import axios, { AxiosRequestConfig, AxiosResponse } from "axios";
import { debounce } from "lodash";

const api = axios.create({
  withCredentials: true,
  baseURL: import.meta.env.VITE_SESSION_URL,
});

const createDebouncedRequest = (delay: number) => {
  return debounce(
    async (
      config: AxiosRequestConfig,
      resolve: (value: AxiosResponse<any>) => void,
      reject: (reason?: any) => void
    ) => {
      try {
        const response = await api.request(config);
        resolve(response);
      } catch (error) {
        reject(error);
      }
    },
    delay
  );
};

export const debouncedFetchSessions = createDebouncedRequest(300);
export const debouncedFetchRegions = createDebouncedRequest(300);

export const makeDebouncedRequest = (
  debouncedFunction: (
    config: AxiosRequestConfig,
    resolve: (value: AxiosResponse<any>) => void,
    reject: (reason?: any) => void
  ) => void,
  config: AxiosRequestConfig
): Promise<AxiosResponse<any>> => {
  return new Promise((resolve, reject) => {
    debouncedFunction(config, resolve, reject);
  });
};
