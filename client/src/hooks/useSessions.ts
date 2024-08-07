import { AxiosResponse } from "axios";
import { useEffect, useState } from "react";

import notFoundBoy from "../assets/404boy.png";
import notFoundGirl from "../assets/404girl.png";
import { useRegion } from "../contexts/RegionContext";
import { debouncedIncrementVisitCount, makeDebouncedRequest as makeDebouncedApiRequest } from "../personalWebsiteApi";
import { debouncedFetchSessions, makeDebouncedRequest as makeDebouncedSessionRequest } from "../sessionsApi";
import { Session } from "../types/Session";

interface AllRegionalSessions {
  region: string;
  newMovieSessions: Session[];
  oldMovieNames: string[];
}

const useSessions = (initialPage: number) => {
  const calculateItemsPerPage = () => {
    // using hard-coded values here since flex box dimensions are inconsistent
    // remember to change the width and height offset if any other elements change lol
    const containerWidth = window.innerWidth - 57;
    const itemWidth = 235; // media card width + horizontal padding
    const columns = Math.floor(containerWidth / itemWidth);
    const containerHeight = window.innerHeight - 280;
    const itemHeight = 150; // media card height + vertical padding
    const rows = Math.floor(containerHeight / itemHeight);

    return columns * rows;
  };

  const [sessions, setSessions] = useState<Session[]>([]);
  const [currentSessions, setCurrentSessions] = useState<Session[]>([]);
  const [currentPage, setCurrentPage] = useState(initialPage);
  const [totalPages, setTotalPages] = useState(0);
  const [itemsPerPage, setItemsPerPage] = useState(() =>
    calculateItemsPerPage()
  );
  const [loading, setLoading] = useState(false);
  const region = useRegion();

  // fill any empty spaces in the flex content area with empty media cards so the positioning isnt scuffed AF
  const emptyMediaCardArray = Array.from(
    {
      length:
        currentPage === totalPages && sessions.length % itemsPerPage !== 0
          ? itemsPerPage - (sessions.length % itemsPerPage)
          : 0,
    },
    (_, index) => index + 1
  );

  useEffect(() => {
    const updateItemsPerPage = () => setItemsPerPage(calculateItemsPerPage());
    updateItemsPerPage();
    window.addEventListener("resize", updateItemsPerPage);

    return () => window.removeEventListener("resize", updateItemsPerPage);
  }, []);

  useEffect(() => {
    setTotalPages(Math.ceil(sessions.length / itemsPerPage));
  }, [itemsPerPage]);

  useEffect(() => {
    setCurrentPage(1);
  }, [region, totalPages]);

  useEffect(() => {
    const getCurrentSessions = () => {
      setCurrentSessions(
        sessions.slice(
          (currentPage - 1) * itemsPerPage,
          currentPage * itemsPerPage
        )
      );
    };

    getCurrentSessions();
  }, [sessions, currentPage, itemsPerPage]);

  useEffect(() => {
    const fetchSessions = () => {
      setLoading(true);
      makeDebouncedSessionRequest(debouncedFetchSessions, {
        url: `/sessions/${region}`,
      })
        .then((response: AxiosResponse<AllRegionalSessions[]>) => {
          const sessions = response.data[0].newMovieSessions;
          sessions.forEach((s) => {
            s.movie.imageUrl = setDefaultImageUrl(s.movie.imageUrl);
          });

          setSessions(sessions);
          setTotalPages(Math.ceil(sessions.length / itemsPerPage));
        })
        .catch((error: any) => {
          setSessions([]);
          console.error("Error fetching sessions:", error);
        })
        .finally(() => setLoading(false));
    };

    fetchSessions();
  }, [region]);

  useEffect(() => {
    makeDebouncedApiRequest(debouncedIncrementVisitCount, {
      url: "/stats/increment",
      method: "post",
    }).catch((error: any) => {
      console.error("Error incrementing visit count:", error);
    });
  }, []);

  const setDefaultImageUrl = (imageUrl: string) => {
    return imageUrl || (Math.random() < 0.5 ? notFoundBoy : notFoundGirl);
  };

  return {
    currentSessions,
    currentPage,
    totalPages,
    itemsPerPage,
    loading,
    emptyMediaCardArray,
    setCurrentPage,
  };
};

export default useSessions;
