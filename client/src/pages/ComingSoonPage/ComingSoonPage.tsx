import styles from "./ComingSoonPage.module.css";

const ComingSoonPage: React.FC = () => (
  <div className={styles.comingSoonPage}>
    <div className={styles.contentArea}>
      <div className={styles.left}>
        <div className={styles.textBox}>
          <h4>Coming Soon</h4>
          <br />
          <p>
            Following industry standards I have decided to release the website
            incomplete without all the features I had promised. They will
            eventually be implemented making this the biggest comeback in the
            industry.
          </p>
          <p>
            In the meantime, keep an eye on this page for what's to come or
            check out the{" "}
            <a
              href="https://github.com/futuramafamilyguy/personal-website"
              target="_blank"
              className={styles.externalLink}
            >
              GitHub repo
            </a>{" "}
            to view my progress. If I've been diligent, this page should be
            empty and you are most welcome to enjoy this picture of Lady Bird
            thrifting with her mum in Sacramento.
          </p>
        </div>
      </div>
      <div className={styles.right}>
        <div className={styles.textBox}>
          <h5>Blog</h5>
          <p>
            A space dedicated to my hot takes on media. Here are some sneak
            peeks:{" "}
            <span style={{ fontStyle: "italic" }}>
              "Shawshank Redemption is one of the best pictures released in the
              last 100 years"
            </span>
            ,{" "}
            <span style={{ fontStyle: "italic" }}>
              "Martin Scorsese is a hidden gem of a director"
            </span>
            ,{" "}
            <span style={{ fontStyle: "italic" }}>
              "Sick of a lack of original content from Hollywood"
            </span>
            .
          </p>
        </div>
        <div className={styles.textBox}>
          <h5>NZ Picture Sessions</h5>
          <p>
            If you enjoy watching older classics or rereleases at the cinemas
            (like me, as you can tell from my Letterbox account) then this one
            is for you. It's easy keeping on top of new releases but knowing
            when your local cinemas are showing reruns can be a bit more tricky.
            So I built a tool that provides weekly updates of all the pictures
            that are playing in cinemas around you including showtimes that
            week. The updates are currently sent in the form of emails but I
            will provide an outlet for all the{" "}
            <a
              href="https://session-tracker.allenmaygibson.com/sessions"
              target="_blank"
              className={styles.externalLink}
            >
              session data
            </a>{" "}
            in a separate page on this website.
          </p>
          <p>
            Since I'm based in Auckland, NZ (and Christchuch at one point), this
            tool is unfortunately limited to cinemas in the Auckland and
            Canterbury regions.
          </p>
        </div>
      </div>
    </div>
  </div>
);

export default ComingSoonPage;
