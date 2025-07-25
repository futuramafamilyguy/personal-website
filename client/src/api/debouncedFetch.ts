import axios, { AxiosRequestConfig, AxiosResponse } from "axios";
import { debounce } from "lodash";

const api = axios.create({
  withCredentials: true,
  baseURL: import.meta.env.VITE_SERVER_URL,
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

export const debouncedFetchStats = createDebouncedRequest(300);

export const debouncedFetchMovies = createDebouncedRequest(300);
export const debouncedFetchActiveYears = createDebouncedRequest(300);
export const debouncedFetchCinemas = createDebouncedRequest(300);

export const debouncedFetchPosts = createDebouncedRequest(300);
export const debouncedFetchPostBySlug = createDebouncedRequest(300);

export const debouncedDisableTracking = createDebouncedRequest(300);
export const debouncedIncrementVisitCount = createDebouncedRequest(300);

export const debouncedLogin = createDebouncedRequest(300);
export const debouncedLogout = createDebouncedRequest(300);

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
