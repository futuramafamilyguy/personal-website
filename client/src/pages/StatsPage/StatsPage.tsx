import { AxiosResponse } from "axios";
import { useEffect, useState } from "react";
import {
  debouncedFetchStats,
  makeDebouncedRequest,
} from "../../personalWebsiteApi";
import styles from "./StatsPage.module.css";

interface Stats {
  totalVisits: number;
  latestVisitUtc: Date | null;
  trackingStartUtc: Date;
}

function StatsPage() {
  const [stats, setStats] = useState<Stats | null>(null);

  useEffect(() => {
    const fetchStats = () => {
      makeDebouncedRequest(debouncedFetchStats, { url: `/stats` })
        .then((response: AxiosResponse<Stats>) => {
          setStats(response.data);
        })
        .catch((error: any) => {
          console.error("Error fetching data:", error);
        });
    };

    fetchStats();
  }, []);

  function convertToNZTime(isoDateTime: Date | undefined | null): string {
    if (!isoDateTime) {
      return "";
    }

    var dateTime = new Date(isoDateTime);

    const options: Intl.DateTimeFormatOptions = {
      timeZone: "Pacific/Auckland",
      weekday: "short",
      day: "numeric",
      month: "short",
      year: "numeric",
      hour: "numeric",
      minute: "2-digit",
      second: "2-digit",
      hour12: true,
    };

    const nzDateTimeString = dateTime.toLocaleString("en-NZ", options);

    return nzDateTimeString;
  }

  return (
    <div className={styles.statsPage}>
      <div className={styles.textBox}>
        <h3 className={styles.title}>Site Stats</h3>
        <br />
        <div>
          <h5 className={styles.title}>Visits:</h5>{" "}
          <h5>{stats?.totalVisits}</h5>
        </div>
        <div className={styles.stat}>
          <h5 className={styles.title}>Last Visit:</h5>{" "}
          <h5>{convertToNZTime(stats?.latestVisitUtc)} (NZT)</h5>
        </div>
        <div className={styles.stat}>
          <h5 className={styles.title}>Since:</h5>{" "}
          <h5>{convertToNZTime(stats?.trackingStartUtc)} (NZT)</h5>
        </div>
      </div>
    </div>
  );
}

export default StatsPage;
