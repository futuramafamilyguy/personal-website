import { useEffect, useState } from "react";

import QstFmContainer from "../../components/QstFm/QstFmContainer";
import { Track } from "../../types/Radio";
import styles from "./QstFmPage.module.css";

import { segments } from "../../data/segments";

interface Segment {
  cover: string;
  intro: string;
  playlist: Track[];
}

function QstFmPage() {
  const [segment, setSegment] = useState<Segment>(segments.roddy);

  useEffect(() => {
    const hour = new Date().getHours();

    if (hour >= 6 && hour < 18) {
      setSegment(segments.roddy);
    } else {
      setSegment(segments.turtledoves);
    }
  }, []);

  return (
    <div
      className={styles.qstFmPage}
      style={{
        ["--bg-image" as any]: `url(${segment.cover})`,
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
