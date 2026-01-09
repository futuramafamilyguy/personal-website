import { useEffect, useState } from "react";

import QstFmContainer from "../../components/QstFm/QstFmContainer";
import { segments } from "../../data/segments";
import { Segment } from "../../types/Radio";
import styles from "./QstFmPage.module.css";
import { useIsMobile } from "../../hooks/useIsMobile";

function QstFmPage() {
  const [segment] = useState<Segment>(() => {
    const hour = new Date().getHours();
    return hour >= 1 && hour < 6
      ? segments.hoothoot
      : hour >= 6 && hour < 10
      ? segments.breakfast
      : hour >= 10 && hour < 16
      ? segments.roddy
      : hour >= 16 && hour < 18
      ? segments.vitaly
      : segments.turtledoves;
  });

  const [cover] = useState<string | undefined>(() => {
    if (!segment.covers || segment.covers.length === 0) return undefined;
    return segment.covers[Math.floor(Math.random() * segment.covers.length)];
  });

  function formatTime(d: Date) {
    return d
      .toLocaleString("en-NZ", {
        timeZone: "Pacific/Auckland",
        hour: "2-digit",
        minute: "2-digit",
        hour12: false,
      })
      .toUpperCase();
  }

  const [time, setTime] = useState(() => formatTime(new Date()));

  useEffect(() => {
    const update = () => setTime(formatTime(new Date()));

    const now = new Date();
    const msToNextMinute =
      (60 - now.getSeconds()) * 1000 - now.getMilliseconds();

    const timeout = setTimeout(() => {
      update();
      const interval = setInterval(update, 60_000);
      return () => clearInterval(interval);
    }, msToNextMinute);

    return () => clearTimeout(timeout);
  }, []);

  const isMobile = useIsMobile();

  if (isMobile) {
    return (
      <div className={styles.qstFmPage}>
        <div className={styles.time}>
          it's now <strong>{time}</strong> on queen street
        </div>
        <div className={styles.imageContainer}>
          <img src={cover} />
        </div>

        <QstFmContainer playlist={segment.playlist} />
        <div className={styles.descriptionBox}>
          <h5>{segment.intro}</h5>
        </div>
      </div>
    );
  }

  return (
    <div
      className={styles.qstFmPage}
      style={{
        ["--bg-image" as any]: `url(${cover})`,
      }}
    >
      <div className={styles.centeredContainer}>
        <QstFmContainer playlist={segment.playlist} />
        <div className={styles.descriptionBox}>
          <h5>
            <i>{segment.intro}</i>
          </h5>
        </div>
      </div>
    </div>
  );
}

export default QstFmPage;
