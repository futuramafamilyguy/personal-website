import { useState } from "react";

import QstFmContainer from "../../components/QstFm/QstFmContainer";
import { segments } from "../../data/segments";
import { Segment } from "../../types/Radio";
import styles from "./QstFmPage.module.css";

function QstFmPage() {
  const [segment] = useState<Segment>(() => {
    const hour = new Date().getHours();
    return hour >= 1 && hour < 6
      ? segments.hoothoot
      : hour >= 6 && hour < 16
      ? segments.roddy
      : hour >= 16 && hour < 18
      ? segments.vitaly
      : segments.turtledoves;
  });

  const [cover] = useState<string | undefined>(() => {
    if (!segment.covers || segment.covers.length === 0) return undefined;
    return segment.covers[Math.floor(Math.random() * segment.covers.length)];
  });

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
