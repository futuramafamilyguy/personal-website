import { AxiosResponse } from "axios";
import { useEffect, useState } from "react";

import { useRegion } from "../contexts/RegionContext";
import {
  debouncedIncrementVisitCount,
  makeDebouncedRequest as makeDebouncedApiRequest,
} from "../personalWebsiteApi";
import {
  debouncedFetchSessions,
  makeDebouncedRequest as makeDebouncedSessionRequest,
} from "../sessionsApi";
import { Session } from "../types/Session";

interface AllRegionalSessions {
  region: string;
  newMovieSessions: Session[];
  oldMovieNames: string[];
}

const useSessions = () => {
  const [sessions, setSessions] = useState<Session[]>([]);
  const [loading, setLoading] = useState(false);
  const region = useRegion();

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
    return (
      imageUrl ||
      (Math.random() < 0.5
        ? "https://cdn.allenmaygibson.com/images/static/sun.jpg"
        : "https://cdn.allenmaygibson.com/images/static/light.jpg")
    );
  };

  return {
    sessions,
    loading,
  };
};

export default useSessions;
